using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using $safeprojectname$.Modules.Email.Interfaces.Services;
using $safeprojectname$.Modules.Email.Enums;

namespace $safeprojectname$.Modules.Email.Services
{
	public class EmailService : IEmailService
	{
		private IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private void SendEmail(MimeMessage email)
		{
			string? host = _configuration.GetValue<string>("Smtp:Host");
			string? username = _configuration.GetValue<string>("Smtp:Username");
			string? password = _configuration.GetValue<string>("Smtp:Password");

			if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				Log.Error($"{nameof(EmailService)} - {nameof(SendEmail)} - ERROR Send email - Configurations are not valid");

				return;
			}

			try
			{
				using (SmtpClient smtpClient = new SmtpClient())
				{
					smtpClient.Connect(host, 587, false);

					smtpClient.Authenticate(username, password);

					smtpClient.Send(email);

					smtpClient.Disconnect(true);
				}
			}
			catch (Exception ex)
			{
				Log.Error($"{nameof(EmailService)} - {nameof(SendEmail)} - ERROR Send email: {JsonSerializer.Serialize(ex)}");
			}
		}

		public void SendEmail(IEnumerable<string> recipients, string subject, string body, EmailBodyFormats bodyFormat, string? sender = null, string? displayName = null, IEnumerable<string>? carbonCopyRecipients = null, IEnumerable<System.Net.Mail.Attachment>? attachments = null)
		{
			if (string.IsNullOrWhiteSpace(sender))
			{
				sender = _configuration.GetValue<string>("Smtp:FromAddress");

				if (string.IsNullOrWhiteSpace(sender))
				{
					Log.Error($"{nameof(EmailService)} - {nameof(SendEmail)} - ERROR Sender is null");

					return;
				}
			}

			if (string.IsNullOrWhiteSpace(displayName))
			{
				displayName = _configuration.GetValue<string>("Smtp:DisplayName");
			}

			try
			{
				List<MailboxAddress> recipientMailboxAddresses = new List<MailboxAddress>();
				foreach (string recipientEmail in recipients)
				{
					recipientMailboxAddresses.Add(new MailboxAddress(recipientEmail.Split("@")?[0] ?? string.Empty, recipientEmail));
				}

				List<MailboxAddress> fromMailboxAddresses = new List<MailboxAddress>() { new MailboxAddress(displayName, sender) };

				BodyBuilder bodyBuilder = new BodyBuilder()
				{
					HtmlBody = bodyFormat == EmailBodyFormats.Html ? body : null,
					TextBody = bodyFormat == EmailBodyFormats.Text ? body : null,
				};

				if (attachments is not null && !attachments.IsNullOrEmpty())
				{
					foreach (System.Net.Mail.Attachment attachment in attachments)
					{
						bodyBuilder.Attachments.Add(new MimePart()
						{
							Content = new MimeContent(attachment.ContentStream),
							FileName = attachment.Name
						});
					}
				}

				MimeMessage message = new MimeMessage(fromMailboxAddresses, recipientMailboxAddresses, subject, bodyBuilder.ToMessageBody());

				if (carbonCopyRecipients is not null && !carbonCopyRecipients.IsNullOrEmpty())
				{
					foreach (string carbonCopyRecipient in carbonCopyRecipients)
					{
						message.Cc.Add(new MailboxAddress(carbonCopyRecipient.Split("@")?[0] ?? string.Empty, carbonCopyRecipient));
					}
				}

				new Thread(() =>
				{
					Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
					SendEmail(message);
				}).Start();
			}
			catch (Exception ex)
			{
				Log.Error($"{nameof(EmailService)} - {nameof(SendEmail)} - ERROR Send email: {JsonSerializer.Serialize(ex)}");
			}

			return;
		}
	}
}