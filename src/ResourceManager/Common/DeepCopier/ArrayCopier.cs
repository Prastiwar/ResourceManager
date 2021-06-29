using System;

namespace DeepCopy
{
    internal static class ArrayCopier
    {
        internal static T[] CopyArrayRank1<T>(T[] originalArray, CopyContext context)
        {
            if (context.TryGetCopy(originalArray, out object existingCopy))
            {
                return (T[])existingCopy;
            }

            int length = originalArray.Length;
            T[] result = new T[length];
            context.RecordCopy(originalArray, result);
            for (int i = 0; i < length; i++)
            {
                T original = originalArray[i];
                if (original != null)
                {
                    if (context.TryGetCopy(original, out object existingElement))
                    {
                        result[i] = (T)existingElement;
                    }
                    else
                    {
                        T copy = CopierGenerator<T>.Copy(original, context);
                        context.RecordCopy(original, copy);
                        result[i] = copy;
                    }
                }
            }
            return result;
        }

        internal static T[,] CopyArrayRank2<T>(T[,] originalArray, CopyContext context)
        {
            if (context.TryGetCopy(originalArray, out object existingCopy))
            {
                return (T[,])existingCopy;
            }

            int lenI = originalArray.GetLength(0);
            int lenJ = originalArray.GetLength(1);
            T[,] result = new T[lenI, lenJ];
            context.RecordCopy(originalArray, result);
            for (int i = 0; i < lenI; i++)
            {
                for (int j = 0; j < lenJ; j++)
                {
                    T original = originalArray[i, j];
                    if (original != null)
                    {
                        if (context.TryGetCopy(original, out object existingElement))
                        {
                            result[i, j] = (T)existingElement;
                        }
                        else
                        {
                            T copy = CopierGenerator<T>.Copy(original, context);
                            context.RecordCopy(original, copy);
                            result[i, j] = copy;
                        }
                    }
                }
            }
            return result;
        }

        internal static T[] CopyArrayRank1Shallow<T>(T[] array, CopyContext context)
        {
            if (context.TryGetCopy(array, out object existingCopy))
            {
                return (T[])existingCopy;
            }

            int length = array.Length;
            T[] result = new T[length];
            context.RecordCopy(array, result);
            Array.Copy(array, result, length);
            return result;
        }

        internal static T[,] CopyArrayRank2Shallow<T>(T[,] array, CopyContext context)
        {
            if (context.TryGetCopy(array, out object existingCopy))
            {
                return (T[,])existingCopy;
            }

            int lenI = array.GetLength(0);
            int lenJ = array.GetLength(1);
            T[,] result = new T[lenI, lenJ];
            context.RecordCopy(array, result);
            Array.Copy(array, result, array.Length);
            return result;
        }

        internal static T CopyArray<T>(T array, CopyContext context)
        {
            if (context.TryGetCopy(array, out object existingCopy))
            {
                return (T)existingCopy;
            }

            if (!(array is Array originalArray))
            {
                throw new InvalidCastException($"Cannot cast non-array type {array?.GetType()} to Array.");
            }

            Type elementType = array.GetType().GetElementType();

            int rank = originalArray.Rank;
            int[] lengths = new int[rank];
            for (int i = 0; i < rank; i++)
            {
                lengths[i] = originalArray.GetLength(i);
            }

            Array copyArray = Array.CreateInstance(elementType, lengths);
            context.RecordCopy(originalArray, copyArray);

            if (DeepCopier.copyPolicy.IsImmutable(elementType))
            {
                Array.Copy(originalArray, copyArray, originalArray.Length);
            }

            int[] index = new int[rank];
            int[] sizes = new int[rank];
            sizes[rank - 1] = 1;
            for (int k = rank - 2; k >= 0; k--)
            {
                sizes[k] = sizes[k + 1] * lengths[k + 1];
            }
            for (int i = 0; i < originalArray.Length; i++)
            {
                int k = i;
                for (int n = 0; n < rank; n++)
                {
                    int offset = k / sizes[n];
                    k -= offset * sizes[n];
                    index[n] = offset;
                }
                object original = originalArray.GetValue(index);
                if (original != null)
                {
                    if (context.TryGetCopy(original, out object existingElement))
                    {
                        copyArray.SetValue(existingElement, index);
                    }
                    else
                    {
                        object copy = DeepCopier.Copy(originalArray.GetValue(index), context);
                        context.RecordCopy(original, copy);
                        copyArray.SetValue(copy, index);
                    }
                }
            }

            return (T)(object)copyArray;
        }
    }
}