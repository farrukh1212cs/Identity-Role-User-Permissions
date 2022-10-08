using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Utility
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"Template/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public async Task SendOTPEmail(EmailOptions opt, SMTPSetting set)
        {
            opt.Subject = "Forgot Your Password!";
            opt.Body = GetEmailBody("CustomerAssignEmailTemplate");

            await SendEmail(opt, set );
        }

        public EmailService(IOptions<SMTPConfigModel> smtpconfig)
        {
            _smtpConfig = smtpconfig.Value;
        }
        private async Task SendEmail(EmailOptions options, SMTPSetting st)
        {
            MailMessage mail = new MailMessage
            {
                Subject = options.Subject,
                Body = UpdatePlaceHolders(options.Body, options.PlaceHolders),
                From = new MailAddress(st.SenderAddress, st.SenderDisplayName),
                IsBodyHtml = st.IsBodyHTML
            };

            foreach (var item in options.ToEmails)
            {
                mail.To.Add(item);
            }
          

            NetworkCredential networkCredential = new NetworkCredential(st.Email, st.Password);
            SmtpClient client = new SmtpClient
            {
                Host = st.Host,
                Port = st.Port,
                EnableSsl = st.EnableSSL,
                UseDefaultCredentials = st.UseDefaultCredentials,
                Credentials = networkCredential
               
            };
            client.UseDefaultCredentials = false;
            mail.BodyEncoding = Encoding.Default;
            await client.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
