using System.Security.Cryptography;
using System.Text;

namespace Yarnique.Modules.UsersManagement.Application.Authentication.PasswordManagement
{
    public static class PasswordHasher
    {
        public static string CreateSalt()
        {
            const int saltSize = 24;
            var bytes = new byte[saltSize];

            using (var keyGenertor = RandomNumberGenerator.Create())
            {
                keyGenertor.GetBytes(bytes);
                return GetStringFromBytes(bytes);
            }
        }

        public static string GetHash(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return GetStringFromBytes(hashedBytes);
            }
        }

        public static bool IsPasswordMatch(string password, string salt, string hashedPassword)
        {
            var test = GetHash(password, salt);
            return test == hashedPassword;
        }

        private static string GetStringFromBytes(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
        }
    }
}
