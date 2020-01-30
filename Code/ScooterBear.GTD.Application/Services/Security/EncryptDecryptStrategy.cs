using System;
using System.Security.Cryptography;
using System.Text;

namespace ScooterBear.GTD.Application.Services.Security
{
    public interface IEncryptDecryptStrategy
    {
        (string, string) Encrypt(string plainText);
        string Decrypt(string iv, string cipherText);
    }

    public class EncryptDecryptStrategy : IEncryptDecryptStrategy
    {
        private readonly ISecurityConfigurationFactory _securityConfigurationFactory;

        public EncryptDecryptStrategy(ISecurityConfigurationFactory securityConfigurationFactory)
        {
            _securityConfigurationFactory = securityConfigurationFactory ?? throw new ArgumentNullException(nameof(securityConfigurationFactory));
        }

        private RijndaelManaged CreateCipher()
        {
            var keyFromConfig = _securityConfigurationFactory.Get().EmailEncryption;
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.KeySize = 256;
            cipher.BlockSize = 128;
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Mode = CipherMode.CBC;
            byte[] key = HexToByteArray(keyFromConfig);
            cipher.Key = key;
            return cipher;
        }

        private byte[] HexToByteArray(string hexString)
        {
            if (0 != (hexString.Length % 2))
            {
                throw new ApplicationException("Hex string must be multiple of 2 in length");
            }

            int byteCount = hexString.Length / 2;
            byte[] byteValues = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                byteValues[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return byteValues;
        }

        public (string, string) Encrypt(string plainText)
        {
            RijndaelManaged rijndael = CreateCipher();
            ICryptoTransform cryptoTransform = rijndael.CreateEncryptor();
            var iv = Convert.ToBase64String(rijndael.IV);
            byte[] plain = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherText = cryptoTransform.TransformFinalBlock(plain, 0, plain.Length);
            var text = Convert.ToBase64String(cipherText);
            return (iv, text);
        }

        public string Decrypt(string iv, string cipherText)
        {
            RijndaelManaged cipher = CreateCipher();
            cipher.IV = Convert.FromBase64String(iv);
            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] plainText = cryptTransform.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);
            return Encoding.UTF8.GetString(plainText);
        }
    }
}
