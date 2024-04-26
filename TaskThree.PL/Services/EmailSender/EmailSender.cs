using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TaskThree.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public async Task SendAsync(string from, string recipients, string subject, string body)
		{
			var SenderEmail = _configuration["EmailSettings:SenderEmail"];
			var SenderPassword = _configuration["EmailSettings:SenderPassword"];
			var emailMessage = new MailMessage();
			emailMessage.From = new MailAddress(from);
			emailMessage.To.Add(recipients);
			emailMessage.Subject = subject;
			emailMessage.Body = $"<html><body>{body}</body></html>";
			emailMessage.IsBodyHtml = true;
			var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpClientServer"],int.Parse (_configuration["EmailSettings:SmtpClientPort"]))
			{
				Credentials = new NetworkCredential(SenderEmail, SenderPassword),
				EnableSsl = true
			};
			await smtpClient.SendMailAsync(emailMessage);
		}
	}
}
