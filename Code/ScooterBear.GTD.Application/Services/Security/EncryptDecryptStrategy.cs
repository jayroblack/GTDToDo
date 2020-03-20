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
            _securityConfigurationFactory = securityConfigurationFactory ??
                                            throw new ArgumentNullException(nameof(securityConfigurationFactory));
        }

        public (string, string) Encrypt(string plainText)
        {
            var rijndael = CreateCipher();
            var cryptoTransform = rijndael.CreateEncryptor();
            var iv = Convert.ToBase64String(rijndael.IV);
            var plain = Encoding.UTF8.GetBytes(plainText);
            var cipherText = cryptoTransform.TransformFinalBlock(plain, 0, plain.Length);
            var text = Convert.ToBase64String(cipherText);
            return (iv, text);
        }

        public string Decrypt(string iv, string cipherText)
        {
            var cipher = CreateCipher();
            cipher.IV = Convert.FromBase64String(iv);
            var cryptTransform = cipher.CreateDecryptor();
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var plainText = cryptTransform.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        private RijndaelManaged CreateCipher()
        {
            var keyFromConfig = _securityConfigurationFactory.Get().EmailEncryption;
            var cipher = new RijndaelManaged();
            cipher.KeySize = 256;
            cipher.BlockSize = 128;
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Mode = CipherMode.CBC;
            var key = HexToByteArray(keyFromConfig);
            cipher.Key = key;
            return cipher;
        }

        private byte[] HexToByteArray(string hexString)
        {
            if (0 != hexString.Length % 2) throw new ApplicationException("Hex string must be multiple of 2 in length");

            var byteCount = hexString.Length / 2;
            var byteValues = new byte[byteCount];
            for (var i = 0; i < byteCount; i++) byteValues[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return byteValues;
        }
    }
}