using System;
using System.Runtime.InteropServices;
using System.Security;

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
    }
}
