﻿namespace BeautifyMe.Services.Interfaces
{
    public interface IEmailService
    {
        public void SendEmail(string email, string subject, string body);
    }
}
