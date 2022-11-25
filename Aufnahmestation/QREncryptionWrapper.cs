using HisDemo.Crypt;

namespace HisDemo.Aufnahmestation
{
    static class QREncryptionWrapper
    {
        // AES 256 Key: THIS IS A DUMMY KEY! WOULD NEED TO BE REPLACED AND SECURELY STORED FOR ACTUAL APPLICATION
        private static byte[] sharedSecretKey =
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
            0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F
        };

        // Short for "MCA Crypt"
        private static string encryptionPrefix = "MCRY-";

        private static SymmetricStringEncryption ses = null;

        private static void InitSES()
        {
            if (ses == null)
                ses = new SymmetricStringEncryption(sharedSecretKey);
        }

        /// <summary>
        /// Take the regular QR-Code data and (if not software-disabled) encrypt it.
        /// </summary>
        /// <param name="plainData">The regular "S-XXXX-YYY-AUXDATA"-Style code.</param>
        /// <returns></returns>
        public static string EncryptQRDataIfEnabled(string plainData)
        {
            if (Program.Config.Software.Failsafe_DoNotEncryptQRCodes)
                return plainData;
            InitSES();
            return encryptionPrefix + ses.Encrypt(plainData);
        }

        /// <summary>
        /// Try to decode the QR code if there is a known encryption prefix. Else, simply pass the data for fallback.
        /// </summary>
        /// <param name="rawData">The raw (ascii) QR-Content</param>
        /// <returns>The decrypted content or the same as rawData input.</returns>
        public static string DecryptOrPassQRData(string rawData)
        {
            if (rawData != null && rawData.StartsWith(encryptionPrefix))
            {
                InitSES();
                return ses.Decrypt(rawData.Substring(encryptionPrefix.Length));
            } else
            {
                return rawData;
            }
        }
    }
}
