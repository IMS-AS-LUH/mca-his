using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HisDemo.Crypt
{
    /// <summary>
    /// This class provies an abstraction layer to easily handle encryption and decryption
    /// of regular strings (e.g. for QR-Code data).
    /// For this class, crypto is based on AES-CBC (cipher block chaining) with the first block containing random data.
    /// That way, the first chiper block is the IV for the second block which contains actual data. The first block is discarded.
    /// For implementation it does not matter if the first block of the chipertext is used as IV and the decryptor starts with the
    /// second block, or if the IV is set to zero, the first block is decrypted wrongly but the second block is correct.
    /// Using this approach ist most flexible to use with any system and does not require a separate IV exchange.
    /// </summary>
    public class SymmetricStringEncryption
    {
        private byte[] key = null;
        private RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public SymmetricStringEncryption(byte[] key = null)
        {
            this.key = key;
        }

        public string Encrypt(string plaintext)
        {
            if (key == null)
                throw new Exception("Key is not set on instance.");
            return Encrypt(key, plaintext);
        }
        public string Decrypt(string chiperTextBase64)
        {
            if (key == null)
                throw new Exception("Key is not set on instance.");
            return Decrypt(key, chiperTextBase64);
        }

        public string Encrypt(byte[] key, string plaintext)
        {
            int blockSize = 16;
            byte[] iv = new byte[blockSize];
            byte[] rb = new byte[blockSize];
            rng.GetBytes(iv);
            rng.GetBytes(rb);
            using (AesManaged aes = new AesManaged()
            {
                Mode = CipherMode.CBC,
                Key = key,
                IV = iv,
            })
            {
                var encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(rb, 0, blockSize);
                        byte[] data = Encoding.UTF8.GetBytes(plaintext);
                        cs.Write(data, 0, data.Length);
                        int pad = blockSize - (data.Length % blockSize);
                        if (pad == blockSize) pad = 0;
                        byte[] zeros = new byte[blockSize];
                        cs.Write(zeros, 0, pad);
                        cs.Flush();
                    }
                    return Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
                }
            }
        }

        public string Decrypt(byte[] key, string chiperTextBase64)
        {
            int blockSize = 16;
            byte[] dummy = new byte[blockSize];
            using (AesManaged aes = new AesManaged()
            {
                Mode = CipherMode.CBC,
                Key = key,
                IV = dummy
            })
            {
                var encryptor = aes.CreateDecryptor(key, dummy);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(chiperTextBase64)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                    {
                        cs.Read(dummy, 0, blockSize); // dummy read
                        byte[] data = new byte[ms.Length - blockSize];
                        cs.Read(data, 0, data.Length);
                        return Encoding.UTF8.GetString(data).TrimEnd('\0');
                    }
                }
            }
        }
    }
}
