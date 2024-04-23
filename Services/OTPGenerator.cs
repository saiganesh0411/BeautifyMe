using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using System;
using Twilio.Rest.Video.V1.Room.Participant;

namespace BeautifyMe.Services
{
    public class OTPGenerator: IOTPGenerator
    {
        private readonly BeautifyMeContext _context;
        private IConfiguration _config;

        public OTPGenerator(BeautifyMeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string GenerateOTP(string userId)
        {
            var constants = _config.GetSection("Constants");
            var guid = Guid.NewGuid();
            var otp = GenerateRandomNumber();
            var otpDetail = new Otpdetail()
            {
                Id = guid,
                ExpiryTime = DateTime.Now.AddMinutes(Convert.ToInt32(constants["OtpValidTimeInMinutes"])),
                IsActive = true,
                Otp = otp,
                UserId = userId
            };

            CleanAndCreateOtp(otpDetail);
            return otp;
        }

        public bool ValidateOTP(string userId, string otp)
        {
            // get existing otp record if exists
            var otpDetail = _context.Otpdetails.Where(otpDetail => otpDetail.UserId == userId && otpDetail.Otp == otp).FirstOrDefault();   
            if(otpDetail != null) 
            {
                if(otpDetail.IsActive && otpDetail.ExpiryTime >= DateTime.Now)
                {
                    otpDetail.IsActive = false;
                    _context.Otpdetails.Update(otpDetail);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CleanAndCreateOtp(Otpdetail otpDetail)
        {
            // remove expired otps
            var removeOtps = _context.Otpdetails.Where(otp => otp.ExpiryTime < DateTime.Now).ToList();
            _context.Otpdetails.RemoveRange(removeOtps);

            // remove otp if user already has an active otp
            var userValidOtps = _context.Otpdetails.Where(otpDetail => otpDetail.UserId == otpDetail.UserId).ToList();
            _context.Otpdetails.RemoveRange(userValidOtps);

            // add the new otp
            _context.Otpdetails.Add(otpDetail);
            _context.SaveChanges();
        }
        public string GenerateRandomNumber()
        {
            var chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
