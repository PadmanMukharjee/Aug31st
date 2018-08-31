using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;

namespace M3Pact.Infrastructure
{
    public class CustomPasswordHasher : IPasswordHasher
    {
        #region Public Methods
        public string HashPassword(string password)
        {
            using (SHA256 mySHA256 = SHA256Managed.Create())
            {
                byte[] hash = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password.ToString()));

                StringBuilder hashSB = new StringBuilder();
                foreach (byte t in hash)
                {
                    hashSB.Append(t.ToString("x2"));
                }
                return hashSB.ToString();
            }
        }


        public PasswordVerificationResult VerifyHashedPassword(
          string hashedPassword, string providedPassword)
        {
            if (hashedPassword == HashPassword(providedPassword))
                return PasswordVerificationResult.Success;
            else
                return PasswordVerificationResult.Failed;
        }
        #endregion
    }
}
