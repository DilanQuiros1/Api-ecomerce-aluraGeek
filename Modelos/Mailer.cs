using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ApiProyecto.Modelos
{
    public class Mailer
    {

       private string key = "youApiKey";

        public async System.Threading.Tasks.Task enviarEmailNuevoPasswd(string email, string nombre, string tok_token)
            {
                string plantillaPath = "Plantillas/correo.cshtml";
                string plantillaHtml = System.IO.File.ReadAllText(plantillaPath);
                     
                nombre = "usuario de alura Store";
                plantillaHtml = plantillaHtml.Replace("#nombre#", nombre);
                plantillaHtml = plantillaHtml.Replace("#correo#", email);
                plantillaHtml = plantillaHtml.Replace("#token#", tok_token);

                var apiKey = key;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("emavalenciano@gmail.com", "Reset Password");
                var subject = "Solicitud de nueva clave";
                var to = new EmailAddress(email, nombre);
                var plainTextContent = "";
                var htmlContent = plantillaHtml;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);//htmlContent
                var response = await client.SendEmailAsync(msg);

            } 
        
        
            public async System.Threading.Tasks.Task envio_correo(string email, string nombre, string mensaje)
            {
                string plantillaPath = "Plantillas/correo_reporte.cshtml";
                string plantillaHtml = System.IO.File.ReadAllText(plantillaPath);

                
                plantillaHtml = plantillaHtml.Replace("#nombre#", nombre);
                plantillaHtml = plantillaHtml.Replace("#msj#", mensaje);

                var apiKey = key;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("emavalenciano@gmail.com", "Alura Store");
                var subject = mensaje;
                var to = new EmailAddress(email, nombre);
                var plainTextContent = "";
                var htmlContent = plantillaHtml;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);//htmlContent
                var response = await client.SendEmailAsync(msg);

            }
        
            public async System.Threading.Tasks.Task envio_correo_compra(string email, string nombre_pro, string precio, string cantidad)
            {
                string plantillaPath = "Plantillas/correo_compra.cshtml";
                string plantillaHtml = System.IO.File.ReadAllText(plantillaPath);

                
                plantillaHtml = plantillaHtml.Replace("#nombre#", nombre_pro);
                plantillaHtml = plantillaHtml.Replace("#precio#", precio);
                plantillaHtml = plantillaHtml.Replace("#cantidad#", cantidad);

                var apiKey = key;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("emavalenciano@gmail.com", "Alura Store");
                var subject = nombre_pro;
                var to = new EmailAddress(email, "Usuario Latam");
                var plainTextContent = "";
                var htmlContent = plantillaHtml;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);//htmlContent
                var response = await client.SendEmailAsync(msg);

            } 
        
          

        

    }
}
