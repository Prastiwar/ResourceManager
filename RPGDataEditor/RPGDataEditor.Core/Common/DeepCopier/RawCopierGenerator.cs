using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
// ReSharper disable StaticMemberInGenericType

namespace DeepCopy
{
    /// <summary>
    /// Generates copy delegates.
    /// </summary>
    internal static class RawCopierGenerator
    {
        private static readonly ConcurrentDictionary<Type, DeepCopyDelegateHandler<object>> copiers = new ConcurrentDictionary<Type, DeepCopyDelegateHandler<object>>();
        private static readonly Func<Type, DeepCopyDelegateHandler<object>> generateCopier = CreateCopier;

        public static object Copy(object original, Type toType, CopyContext context)
        {
            // ReSharper disable once ExpressionIsAlwaysNull
            if (original == null)
            {
                return original;
            }
            DeepCopyDelegateHandler<object> result = copiers.GetOrAdd(toType, generateCopier);
            return result(original, context);
        }

        /// <summary>
        /// Gets a copier for the provided type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A copier for the provided type.</returns>
        private static DeepCopyDelegateHandler<object> CreateCopier(Type type)
        {
            if (type.IsArray)
            {
                return CreateArrayCopier(type);
            }

            if (DeepCopier.copyPolicy.IsImmutable(type))
            {
                return (original, context) => original;
            }

            // By-ref types are not supported.
            if (type.IsByRef)
            {
                return ThrowNotSupportedType(type);
            }

            DynamicMethod dynamicMethod = new DynamicMethod(
                type.Name + "DeepCopier",
                typeof(object),
                new[] { typeof(object), typeof(CopyContext) },
                typeof(DeepCopier).Module,
                true);

            ILGenerator il = dynamicMethod.GetILGenerator();

            // Declare a variable to store the result.
            il.DeclareLocal(type);

            bool needsTracking = DeepCopier.copyPolicy.NeedsTracking(type);
            Label hasCopyLabel = il.DefineLabel();
            if (needsTracking)
            {
                // C#: if (context.TryGetCopy(original, out object existingCopy)) return (T)existingCopy;
                il.DeclareLocal(typeof(object));
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloca_S, (byte)1);
                il.Emit(OpCodes.Call, DeepCopier.methodInfos.TryGetCopy);
                il.Emit(OpCodes.Brtrue, hasCopyLabel);
            }

            // Construct the result.
            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (type.IsValueType)
            {
                // Value types can be initialized directly.
                il.Emit(OpCodes.Ldloca_S, (byte)0);
                il.Emit(OpCodes.Initobj, type);
            }
            else if (constructorInfo != null)
            {
                // If a default constructor exists, use that.
                il.Emit(OpCodes.Newobj, constructorInfo);
                il.Emit(OpCodes.Stloc_0);
            }
            else
            {
                // If no default constructor exists, create an instance using GetUninitializedObject
                il.Emit(OpCodes.Ldtoken, type);
                il.Emit(OpCodes.Call, DeepCopier.methodInfos.GetTypeFromHandle);
                il.Emit(OpCodes.Call, DeepCopier.methodInfos.GetUninitializedObject);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Stloc_0);
            }

            // An instance of a value types can never appear multiple times in an object graph,
            // so only record reference types in the context.
            if (needsTracking)
            {
                // Record the object.
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Call, DeepCopier.methodInfos.RecordObject);
            }

            // Copy each field.
            foreach (FieldInfo field in DeepCopier.copyPolicy.GetCopyableFields(type))
            {
                // Load a reference to the result.
                if (type.IsValueType)
                {
                    // Value types need to be loaded by address rather than copied onto the stack.
                    il.Emit(OpCodes.Ldloca_S, (byte)0);
                }
                else
                {
                    il.Emit(OpCodes.Ldloc_0);
                }

                // Load the field from the result.
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);

                // Deep-copy the field if needed, otherwise just leave it as-is.
                if (!DeepCopier.copyPolicy.IsImmutable(field.FieldType))
                {
                    // Copy the field using the generic DeepCopy.Copy<object> method.
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Call, DeepCopier.methodInfos.CopyInner.MakeGenericMethod(field.FieldType));
                }

                // Store the copy of the field on the result.
                il.Emit(OpCodes.Stfld, field);
            }

            // Return the result.
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            if (needsTracking)
            {
                // only non-ValueType needsTracking
                il.MarkLabel(hasCopyLabel);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Ret);
            }

            return dynamicMethod.CreateDelegate(typeof(DeepCopyDelegateHandler<object>)) as DeepCopyDelegateHandler<object>;
        }

        private static DeepCopyDelegateHandler<object> CreateArrayCopier(Type type)
        {
            Type elementType = type.GetElementType();

            int rank = type.GetArrayRank();
            bool isImmutable = DeepCopier.copyPolicy.IsImmutable(elementType);
            MethodInfo methodInfo;
            switch (rank)
            {
                case 1:
                    if (isImmutable)
                    {
                        methodInfo = DeepCopier.methodInfos.CopyArrayRank1Shallow;
                    }
                    else
                    {
                        methodInfo = DeepCopier.methodInfos.CopyArrayRank1;
                    }
                    break;
                case 2:
                    if (isImmutable)
                    {
                        methodInfo = DeepCopier.methodInfos.CopyArrayRank2Shallow;
                    }
                    else
                    {
                        methodInfo = DeepCopier.methodInfos.CopyArrayRank2;
                    }
                    break;
                default:
                    return ArrayCopier.CopyArray;
            }

            return (DeepCopyDelegateHandler<object>)methodInfo.MakeGenericMethod(elementType).CreateDelegate(typeof(DeepCopyDelegateHandler<object>));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static DeepCopyDelegateHandler<object> ThrowNotSupportedType(Type type) => throw new NotSupportedException($"Unable to copy object of type {type}.");
    }
}
