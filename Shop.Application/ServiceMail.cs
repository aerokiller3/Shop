using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Shop.Application
{
    public class ServiceMail
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "kirillceeb@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("",email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                //TODO: Извлекать пароль из файла конфигурации
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("kirillceeb@gmail.com", "ZDWF230899rt");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
