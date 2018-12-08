using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternalApi.Security
{
    public class RegHash
    {
            private const int SaltLen = 0;
            private const int IterationCount = 10000;

            public string RegEncrypt(string password)
            {
                
                //if (password == null)
                //{
                //    throw new ArgumentNullException("Password cannot be null");
                //}
                var saltBytes = GetSalt(SaltLen);
                var hash = Hash(password, saltBytes);
                return Convert.ToBase64String(saltBytes) + "$" + Convert.ToBase64String(hash);
            }

            public bool Matches(string password, string hashed)
            {
                var parts = hashed.Split('$');
                if (parts.Length != 2)
                {
                    return false;
                }

                var salt = parts[0];
                var hash = parts[1];
                var computed = Hash(password, Convert.FromBase64String(salt));
                return Convert.ToBase64String(computed) == hash;
            }

            private byte[] Hash(string password, byte[] salt)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    IterationCount))
                {
                    return pbkdf2.GetBytes(32);
                }
            }

            private byte[] GetSalt(int maximumSaltLength)
            {
                var salt = new byte[maximumSaltLength];
                using (var random = new RNGCryptoServiceProvider())
                {
                    random.GetNonZeroBytes(salt);
                }

                return salt;
            }
    }
}