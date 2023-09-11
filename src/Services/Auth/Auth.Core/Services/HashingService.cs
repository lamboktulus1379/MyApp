using Auth.Core.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace Auth.Core.Services
{
    public class HashingService : IHashing
    {
        public string Generate(string password)
        {

            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            //using (var rngCsp = new RSACryptoServiceProvider())
            //{
            //    object p = rngCsp.GetBytes(salt);
            //}
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            //Console.WriteLine($"Hashed: {hashed}");

            return hashed;
        }
    }
}
