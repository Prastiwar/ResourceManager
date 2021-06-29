using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ResourceManager
{
    public static class PrimitiveExtensions
    {
        public static SecureString ToSecure(this string secret)
        {
            SecureString secureString;
            if (secret == null || secret.Length == 0)
            {
                return new SecureString();
            }
            unsafe
            {
                fixed (char* pch = secret)
                {
                    secureString = new SecureString(pch, secret.Length);
                }
            }
            return secureString;
        }

        public static string ToUnsecure(this SecureString secure)
        {
            string plainString;
            IntPtr bstr = IntPtr.Zero;
            if (secure == null || secure.Length == 0)
            {
                return string.Empty;
            }

            try
            {
                bstr = Marshal.SecureStringToBSTR(secure);
                plainString = Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                if (bstr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr);
                }
            }
            return plainString;
        }

        public static string ToFirstLower(this string value) => char.ToLower(value[0]) + value[1..];

        public static object DeepClone(this object obj, Type toType = null) => DeepCopy.DeepCopier.Copy(obj, toType ?? obj.GetType());

        public static T DeepClone<T>(this T obj, Type toType = null) => (T)DeepCopy.DeepCopier.Copy(obj, toType ?? typeof(T));

        public static void CopyProperties(this object toCopy, object fromCopy) => CopyRelatedProperties(toCopy, fromCopy, (prop) => {
            PropertyInfo fromCopyProp = fromCopy.GetType().GetProperty(prop.Name);
            bool canSetValue = prop.SetMethod != null && fromCopyProp != null && fromCopyProp.PropertyType == prop.PropertyType;
            bool canGetValue = fromCopyProp.GetMethod != null;
            if (canSetValue && canGetValue)
            {
                object value = fromCopyProp.GetValue(fromCopy);
                prop.SetValue(toCopy, value);
            }
        });

        private static bool CopyRelatedProperties(object toObject, object fromCopy, Action<PropertyInfo> copyProperty)
        {
            try
            {
                bool hasCopyRelatioship = toObject.GetType().IsAssignableFrom(fromCopy.GetType()) || fromCopy.GetType().IsAssignableFrom(toObject.GetType());
                if (!hasCopyRelatioship)
                {
                    return false;
                }
                PropertyInfo[] props = toObject.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    copyProperty(prop);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static readonly char[] braces = new char[] {
            '[', ']', '(', ')', '{', '}'
        };

        public static string MakeFriendlyName(this string text, bool preserveBraces = false, bool preserveAcronyms = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(text.Length * 2);
            if (preserveBraces || !braces.Any(brace => brace == text[0]))
            {
                builder.Append(text[0]);
            }
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    char previousChar = text[i - 1];
                    if ((previousChar != ' ' && !char.IsUpper(previousChar)) ||
                        (preserveAcronyms && char.IsUpper(previousChar) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        builder.Append(' ');
                    }
                }
                builder.Append(text[i]);
            }
            if (!preserveBraces && braces.Any(brace => brace == builder[builder.Length - 1]))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }
    }
}
