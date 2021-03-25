using RPGDataEditor.Core.Models;
using System;
using System.Linq;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultModelProvider<TModel> : IModelProvider<TModel> where TModel : ObservableModel
    {
        private static readonly Type[] derivedTypes = typeof(TModel).EnumarateDerivedTypes().ToArray();

        public TModel CreateModel(string name)
        {
            foreach (Type type in derivedTypes)
            {
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
