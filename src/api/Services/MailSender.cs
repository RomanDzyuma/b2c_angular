using api.Configuration;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace api.Services
{
    public class MailSender : IMailSender
    {
        private readonly IOptions<MailOptions> _options;

        public MailSender(IOptions<MailOptions> options)
        {
            _options = options;
        }

        public async Task InviteAsync(string toEmail, string toName, string token)
        {
            var client = new SendGridClient(_options.Value.Secret);
            var from = new EmailAddress(_options.Value.From.Email, _options.Value.From.Name);
            var subject = _options.Value.Subject;
            var to = new EmailAddress(toEmail, toName);

            string htmlTemplate;
            using (var reader = new StreamReader(Path.Combine("Templates", "Template.html")))
            {
                htmlTemplate = await reader.ReadToEndAsync();
            }

            var htmlContent = string.Format(htmlTemplate, toEmail, token);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            
            await client.SendEmailAsync(msg);
        }
    }
}
