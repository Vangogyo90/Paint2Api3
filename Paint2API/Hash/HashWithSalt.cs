using System.Security.Cryptography;
using System.Text;

namespace AutoPartsAPI.Hash
{
    public class HashWithSalt
    {
        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            using (SHA256 algorithm = SHA256.Create())
            {
                byte[] plainTextWithSaltBytes =
                    new byte[plainText.Length + salt.Length];

                Buffer.BlockCopy(plainText, 0, plainTextWithSaltBytes, 0, plainText.Length);
                Buffer.BlockCopy(salt, 0, plainTextWithSaltBytes, plainText.Length, salt.Length);

                return algorithm.ComputeHash(plainTextWithSaltBytes);
            }
        }

        public static string Decrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Encrypt(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
