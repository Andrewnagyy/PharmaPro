using System.Security.Cryptography;

namespace PharmaPro.Core.Contract.Identity
{
    public class OTPGenerator : IOTPGenerator
    {
        public string Generate(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            Span<byte> bytes = stackalloc byte[length];
            rng.GetBytes(bytes);

            Span<char> otp = stackalloc char[length];

            for (int i = 0; i < length; i++)
            {
                var digit = bytes[i] % 10;
                otp[i] = (char)('0' + digit);
            }
            return otp.ToString();
        }
    }
}
