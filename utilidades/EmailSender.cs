using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid.Helpers.Mail;
using SendGrid;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiProyecto.utilidades
{
    public class EmailSender : IEmailSender
    {
        //public string SendGridSecret { get; set; }

      
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient("Your-Client-SendGrid");
            var from = new EmailAddress("no-reply@igle.online", "Iglesia en linea");       
            var to = new EmailAddress(email);
            //var htmlContent = cuepohtml;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);
            return client.SendEmailAsync(msg);
        }
    }
}
