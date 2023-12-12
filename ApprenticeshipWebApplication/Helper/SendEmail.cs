using System.Net.Mail;
using System.Text;

namespace ApprenticeshipWebApplication.Helper
{
    public static class SendEmail
    {
        public static void NewEmail(string to, string mailbody, string subject)
        {
            string from = "fromaddress@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("nooralhuda101020@gmail.com", "80052001Na$");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {


            }
        }
    }
}