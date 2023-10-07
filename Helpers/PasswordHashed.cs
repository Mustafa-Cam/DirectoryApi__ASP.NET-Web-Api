using System.Security.Cryptography;
namespace DirectoryApi.Helpers
{
    public class PasswordHashed
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int SaltSize = 17;
        private static readonly int Hashsize = 20;
        private static readonly int Iterations = 1000;

        public static string HashPassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[SaltSize]);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = key.GetBytes(Hashsize);

            var hashBytes = new byte[SaltSize + Hashsize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, Hashsize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = key.GetBytes(Hashsize);

            for (var i = 0; i < Hashsize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
