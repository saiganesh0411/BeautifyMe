using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Humanizer;
using BeautifyMe.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace BeautifyMe.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        //public void SendSms(string phoneNumber, string text)
        //{
        //    var twilioDetails = _config.GetSection("Twilio");
        //    var accountSid = twilioDetails["AccountSid"];
        //    var authToken = twilioDetails["AuthToken"];
        //    TwilioClient.Init(accountSid, authToken);
        //    if(!phoneNumber.StartsWith("+1"))
        //    {
        //        phoneNumber = "+1" + phoneNumber;
        //    }
        //    var messageOptions = new CreateMessageOptions(
        //      new PhoneNumber(phoneNumber));
        //    messageOptions.From = new PhoneNumber(twilioDetails["PhoneNumber"]);
        //    messageOptions.Body = "Message from BeautifyMe: " + text;

        //    var message = MessageResource.Create(messageOptions);
        //}

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var gmailSmtpDetails = _config.GetSection("GmailSmtp");
                var fromEmail = gmailSmtpDetails["FromEmail"];
                var passcode = gmailSmtpDetails["Passcode"];
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = Convert.ToInt32(gmailSmtpDetails["Port"]),
                    Credentials = new NetworkCredential(fromEmail, passcode),
                    EnableSsl = true
                };
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Email not sent", ex);
            }
            
        }
    }
}
