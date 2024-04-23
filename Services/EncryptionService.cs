using BeautifyMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BeautifyMe.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string text, Guid key)
        {
            var encryptionKey = Regex.Replace(key.ToString(), @"[^\w\d]", "");
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] keyArray = new byte[32]; // Ensure the key is 32 bytes long for AES-256
                Array.Copy(keyBytes, keyArray, keyBytes.Length);

                aesAlg.Key = keyArray;
                aesAlg.IV = new byte[16]; // Use an appropriate IV (Initialization Vector) for encryption

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText, Guid key)
        {
            var encryptionKey = Regex.Replace(key.ToString(), @"[^\w\d]", "");
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] keyArray = new byte[32]; // Ensure the key is 32 bytes long for AES-256
                Array.Copy(keyBytes, keyArray, keyBytes.Length);

                aesAlg.Key = keyArray;
                aesAlg.IV = new byte[16]; // Use the same IV used for encryption

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }


        public string MaskCreditCard(string creditCardNumber)
        {
            if (creditCardNumber.Length >= 4)
            {
                int maskedLength = creditCardNumber.Length - 4;
                return new string('*', maskedLength) + creditCardNumber.Substring(maskedLength);
            }
            return creditCardNumber;
        }


        //    private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        //    {
        //        // Check arguments.  
        //        if (cipherText == null || cipherText.Length <= 0)
        //        {
        //            throw new ArgumentNullException("cipherText");
        //        }
        //        if (key == null || key.Length <= 0)
        //        {
        //            throw new ArgumentNullException("key");
        //        }
        //        if (iv == null || iv.Length <= 0)
        //        {
        //            throw new ArgumentNullException("key");
        //        }

        //        // Declare the string used to hold  
        //        // the decrypted text.  
        //        string plaintext = null;

        //        // Create an RijndaelManaged object  
        //        // with the specified key and IV.  
        //        using (var rijAlg = new RijndaelManaged())
        //        {
        //            //Settings  
        //            rijAlg.Mode = CipherMode.CBC;
        //            rijAlg.Padding = PaddingMode.PKCS7;
        //            rijAlg.FeedbackSize = 128;

        //            rijAlg.Key = key;
        //            rijAlg.IV = iv;

        //            // Create a decrytor to perform the stream transform.  
        //            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

        //            try
        //            {
        //                // Create the streams used for decryption.  
        //                using (var msDecrypt = new MemoryStream(cipherText))
        //                {
        //                    using(var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //                    {

        //                        using (var srDecrypt = new StreamReader(csDecrypt))
        //                        {
        //                            // Read the decrypted bytes from the decrypting stream  
        //                            // and place them in a string.  
        //                            plaintext = srDecrypt.ReadToEnd();

        //                        }

        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                plaintext = "keyError";
        //            }
        //        }

        //        return plaintext;
        //    }
        //    private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        //    {
        //        // Check arguments.  
        //        if (plainText == null || plainText.Length <= 0)
        //        {
        //            throw new ArgumentNullException("plainText");
        //        }
        //        if (key == null || key.Length <= 0)
        //        {
        //            throw new ArgumentNullException("key");
        //        }
        //        if (iv == null || iv.Length <= 0)
        //        {
        //            throw new ArgumentNullException("key");
        //        }
        //        byte[] encrypted;
        //        // Create a RijndaelManaged object  
        //        // with the specified key and IV.  
        //        using (var rijAlg = new RijndaelManaged())
        //        {
        //            rijAlg.Mode = CipherMode.CBC;
        //            rijAlg.Padding = PaddingMode.PKCS7;
        //            rijAlg.FeedbackSize = 128;

        //            rijAlg.Key = key;
        //            rijAlg.IV = iv;

        //            // Create a decrytor to perform the stream transform.  
        //            var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

        //            // Create the streams used for encryption.  
        //            using (var msEncrypt = new MemoryStream())
        //            {
        //                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //                {
        //                    using (var swEncrypt = new StreamWriter(csEncrypt))
        //                    {
        //                        //Write all data to the stream.  
        //                        swEncrypt.Write(plainText);
        //                    }
        //                    encrypted = msEncrypt.ToArray();
        //                }
        //            }
        //        }

        //        // Return the encrypted bytes from the memory stream.  
        //        return encrypted;
        //    }
        //    public string DecryptForJavaScript(Guid encryptionCode, string cipherText)
        //    {
        //        if (!string.IsNullOrEmpty(cipherText))
        //        {
        //            byte[] keybytes = encryptionCode.ToByteArray();

        //            var encrypted = Convert.FromBase64String(cipherText);
        //            var decryptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, keybytes);
        //            return string.Format(decryptedFromJavascript);
        //        }
        //        return cipherText;
        //    }

        //    public string EncryptForJavaScript(Guid encryptionCode, string text)
        //    {
        //        byte[] keybytes = encryptionCode.ToByteArray();

        //        // Use the same key for IV
        //        byte[] iv = keybytes;

        //        var encryptedText = EncryptStringToBytes(text, keybytes, iv);
        //        return Convert.ToBase64String(encryptedText);
        //    }

        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using(var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }

        public string DecryptForJavaScript(Guid encryptionCode, string cipherText)
        {
            if(!string.IsNullOrEmpty(cipherText))
            {
                var convertedEncryptionCode = convertEncryptedStringTo16Chars(encryptionCode.ToString());
                var keybytes = Encoding.UTF8.GetBytes(convertedEncryptionCode);
                var iv = Encoding.UTF8.GetBytes(convertedEncryptionCode);

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            return cipherText;
        }

        public string EncryptForJavaScript(Guid encryptionCode, string text)
        {
            var convertedEncryptionCode = convertEncryptedStringTo16Chars(encryptionCode.ToString());
            var keybytes = Encoding.UTF8.GetBytes(convertedEncryptionCode);
            var iv = Encoding.UTF8.GetBytes(convertedEncryptionCode);

            var encryptedText = EncryptStringToBytes(text, keybytes, iv);
            return Convert.ToBase64String(encryptedText);
        }

        private string convertEncryptedStringTo16Chars(string encryptionCode)
        {
            int desiredLength = 16;
            encryptionCode = Regex.Replace(encryptionCode, @"[^\w\d]", "");
            if (encryptionCode.Length < desiredLength)
            {
                encryptionCode = encryptionCode.PadRight(desiredLength, '0');
            }
            else if (encryptionCode.Length > desiredLength)
            {
                encryptionCode = encryptionCode.Substring(0, desiredLength);
            }
            return encryptionCode;
        }
    }
}
