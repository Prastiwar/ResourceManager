using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ResourceManager.Data
{
    public class PathResourceDescriptor : ResourceDescriptor
    {
        private static readonly char[] bracketsArray = new char[] { '{', '}' };

        public PathResourceDescriptor(Type type, string relativeRootPath, string relativeFullPathFormat) : base(type)
        {
            RelativeRootPath = relativeRootPath;
            RelativeFullPathFormat = relativeRootPath + relativeFullPathFormat;
        }

        public string RelativeFullPathFormat { get; protected set; }

        public string RelativeRootPath { get; protected set; }

        public string RelativeFullPathStringFormat {
            get {
                int counter = -1;
                return Regex.Replace(RelativeFullPathFormat, "{[^{}]+}", (match) => {
                    if (match.Success)
                    {
                        counter++;
                        return "{" + counter.ToString() + "}";
                    }
                    return null;
                }, RegexOptions.Compiled);
            }
        }

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
            return string.Format(RelativeFullPathStringFormat, argumentValues);
        }

        public virtual string GetRelativeFullPath(params KeyValuePair<string, object>[] parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            string[] argumentNames = RelativeFullPathFormat.Split(bracketsArray, StringSplitOptions.RemoveEmptyEntries);
            object[] argumentValues = new object[argumentNames.Length];
            for (int i = 0; i < argumentNames.Length; i++)
            {
                KeyValuePair<string, object> argumentValue = parameters.FirstOrDefault(parameter => string.Compare(parameter.Key, argumentNames[i], true) == 0);
                argumentValues[i] = argumentValue.Value;
            }
            return string.Format(RelativeFullPathStringFormat, argumentValues);
        }

        public virtual KeyValuePair<string, object>[] ParseParameters(string path)
        {
            string[] splitFormat = RelativeFullPathFormat.Split(bracketsArray, StringSplitOptions.RemoveEmptyEntries);
            string[] argumentNames = new string[splitFormat.Length / 2];
            int oddSplitFormatIndex = 1;
            for (int i = 0; i < argumentNames.Length; i++)
            {
                argumentNames[i] = splitFormat[oddSplitFormatIndex];
                oddSplitFormatIndex += 2;
            }
            KeyValuePair<string, object>[] parameters = new KeyValuePair<string, object>[argumentNames.Length];
            string format = RelativeFullPathStringFormat;
            string[] argumentValues = new string[format.Count(c => c == '{')];

            if (argumentNames.Length != argumentValues.Length)
            {
                throw new ArgumentException("Path is not compatible with format: " + format, nameof(path));
            }

            for (int i = 0; i < argumentValues.Length; i++)
            {
                int braceIndex = format.IndexOf('{');
                int endBraceIndex = format.IndexOf('}');
                int endDataIndex = 0;
                if (format.Length > endBraceIndex + 1)
                {
                    char matchingNextChar = format[endBraceIndex + 1];
                    endDataIndex = path.IndexOf(matchingNextChar, braceIndex);
                    argumentValues[i] = path.Substring(braceIndex, endDataIndex - braceIndex);
                    path = path.Remove(0, endDataIndex + 1);
                }
                else
                {
                    argumentValues[i] = path.Substring(braceIndex);
                    path = "";
                }

                if (format.Length >= endBraceIndex + 2)
                {
                    format = format.Remove(0, endBraceIndex + 2);
                }
                else
                {
                    format = "";
                }
            }

            for (int i = 0; i < argumentNames.Length; i++)
            {
                parameters[i] = new KeyValuePair<string, object>(argumentNames[i], argumentValues[i]);
            }
            return parameters;
        }
    }
}
