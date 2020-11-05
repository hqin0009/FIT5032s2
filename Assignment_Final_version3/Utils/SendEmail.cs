using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Assignment_Final_version3.Utils
{
    public class SendEmail
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("qinhaoran88@gmail.com", "HaoranQIn");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

    }
}