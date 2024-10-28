using System.Security.Cryptography;
using System.Text;

namespace Application.Extensions
{
    public static class CustomExtension
    {
        public static string GetHash(this string value)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder builder = new();

            for(int i = 0;  i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
