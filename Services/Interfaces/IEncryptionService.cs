namespace BeautifyMe.Services.Interfaces
{
    public interface IEncryptionService
    {
        public string Encrypt(string text, Guid key);
        public string Decrypt(string cipherText, Guid key);
        public string MaskCreditCard(string creditCardNumber);
        public string DecryptForJavaScript(Guid encryptionCode, string cipherText);
        public string EncryptForJavaScript(Guid encryptionCode, string text);
    }
}
