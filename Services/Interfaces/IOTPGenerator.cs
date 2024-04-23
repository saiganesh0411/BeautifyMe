namespace BeautifyMe.Services.Interfaces
{
    public interface IOTPGenerator
    {
        public string GenerateOTP(string userId);
        public bool ValidateOTP(string userId, string otp);
    }
}
