using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace ResourceManager
{
    public static class SecretString
    {
        private const string entropyKey = "ResourceManager.SecretString.Entropy";
        private static byte[] Entropy {
            get {
                string entropy = null;
                try
                {
                    entropy = Environment.GetEnvironmentVariable(entropyKey, EnvironmentVariableTarget.User);
                }
                catch (Exception)
                {
                }
                if (entropy == null)
                {
                    entropy = Guid.NewGuid().ToString();
                    try
                    {
                        Environment.SetEnvironmentVariable(entropyKey, entropy, EnvironmentVariableTarget.User);
                    }
                    catch (Exception)
                    {
                    }
                }
                return Encoding.Unicode.GetBytes(entropy);
            }
        }

        public static string EncryptString(SecureString input, byte[] entropy = null)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input.ToUnsecure()), entropy ?? Entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData, byte[] entropy = null)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy ?? Entropy, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(decryptedData).ToSecure();
            }
            catch (Exception)
            {
                return new SecureString();
            }
        }
    }
}
