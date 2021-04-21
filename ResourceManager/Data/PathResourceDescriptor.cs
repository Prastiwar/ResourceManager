using System;
using System.Reflection;

namespace ResourceManager.Data
{
    public class PathResourceDescriptor : ResourceDescriptor
    {
        private static readonly char[] bracketsArray = new char[] { '{', '}' };

        public PathResourceDescriptor(Type type, string relativeFullPathFormat, string relativeRootPath) : base(type)
        {
            RelativeFullPathFormat = relativeRootPath + "/" + relativeFullPathFormat;
            RelativeRootPath = relativeRootPath;
        }

        public string RelativeFullPathFormat { get; protected set; }

        public string RelativeRootPath { get; protected set; }

        /// <summary> Creates relative full path by filling RelativeFullPathFormat arguments with <paramref name="resource"/> properties </summary>
        /// <param name="resource"> Target object with arguments as properties </param>
        /// <returns> Relative Full Path to <paramref name="resource"/> using <paramref name="RelativeFullPathFormat"/> </returns>
        public virtual string GetRelativeFullPath(object resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            string[] argumentNames = RelativeFullPathFormat.Split(bracketsArray, StringSplitOptions.RemoveEmptyEntries);
            object[] argumentValues = new object[argumentNames.Length];
            for (int i = 0; i < argumentNames.Length; i++)
            {
                PropertyInfo property = resource.GetType().GetProperty(argumentNames[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null && property.GetMethod != null)
                {
                    object argumentValue = property.GetValue(resource);
                    argumentValues[i] = argumentValue;
                }
            }
            return string.Format(RelativeFullPathFormat, argumentValues);
        }
    }
}
