using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Providers
{
    public class DefaultModelProvider<TModel> : IModelProvider<TModel> where TModel: class
    {
        protected static Type[] DerivedTypes { get; } = typeof(TModel).EnumarateDerivedTypes().ToArray();

        protected virtual HashSet<Type> GetIgnoredTypes() => null;

        public virtual TModel CreateModel(string name)
        {
            HashSet<Type> ignoredTypes = GetIgnoredTypes();
            foreach (Type type in DerivedTypes)
            {
                if (ignoredTypes != null && ignoredTypes.Contains(type))
                {
                    continue;
                }
                string typeName = GetTypeNameToCompare(type.Name);
                if (typeName.CompareTo(name) == 0)
                {
                    return Activator.CreateInstance(type) as TModel;
                }
            }
            return default;
        }

        protected virtual string GetTypeNameToCompare(string name) => name;
    }
}
