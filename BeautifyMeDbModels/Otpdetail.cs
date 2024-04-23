using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Otpdetail
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public DateTime ExpiryTime { get; set; }
        public bool IsActive { get; set; }
    }
}
