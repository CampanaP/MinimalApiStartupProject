using Serilog;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;
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

		private void SendEmail(object data)
		{
			MailMessage? emailMessage = data as MailMessage;
			if (emailMessage is null)
			{
				return;
			}

			SmtpClient smtpClient = new SmtpClient();
			string? host = _configuration.GetValue<string>("Smtp:Host");
			string? username = _configuration.GetValue<string>("Smtp:Username");
			string? password = _configuration.GetValue<string>("Smtp:Password");

			if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				return;
			}

			smtpClient.Host = host;
			smtpClient.Credentials = new System.Net.NetworkCredential(username, password);

			try
			{
				smtpClient.Send(emailMessage);
			}
			catch (SmtpException ex)
			{
				Log.Error($"{nameof(EmailService)} - {nameof(SendEmail)} - ERROR Send email: {JsonSerializer.Serialize(ex)}");
			}
		}

		public void SendEmail(IEnumerable<string> recipients, string @object, string body, EmailBodyFormats bodyFormat, string? sender = null, string? displayName = null, IEnumerable<string>? carbonCopyRecipients = null)
		{
			if (string.IsNullOrWhiteSpace(sender))
			{
				sender = _configuration.GetValue<string>("Smtp:FromAddress");
			}

			if (string.IsNullOrWhiteSpace(displayName))
			{
				displayName = "ConnectorProject";
			}

			SendEmail(recipients, @object, body, bodyFormat, null, sender, displayName, carbonCopyRecipients);
		}

		public void SendEmail(IEnumerable<string> recipients, string @object, string body, EmailBodyFormats bodyFormat, List<Attachment>? attachments = null, string? sender = null, string? displayName = null, IEnumerable<string>? carbonCopyRecipients = null)
		{
			if (string.IsNullOrWhiteSpace(sender))
			{
				sender = _configuration.GetValue<string>("Smtp:FromAddress");
			}

			if (string.IsNullOrWhiteSpace(displayName))
			{
				displayName = "ConnectorProject";
			}

			MailMessage message = new MailMessage();

			foreach (string recipientMail in recipients)
			{
				message.To.Add(recipientMail);
			}

			if (string.IsNullOrWhiteSpace(sender))
			{
				Log.Error($"{nameof(EmailService)} - {nameof(SendEmail)} - ERROR Sender is null");

				return;
			}

			MailAddress senderEmail = new MailAddress(sender);

			if (!string.IsNullOrWhiteSpace(displayName))
			{
				senderEmail = new MailAddress(sender, displayName);
			}

			message.IsBodyHtml = bodyFormat == EmailBodyFormats.Html;
			message.Subject = @object;
			message.Body = body;
			message.Sender = senderEmail;
			message.From = senderEmail;

			if (carbonCopyRecipients != null && carbonCopyRecipients.Any())
			{
				foreach (string carbonCopyRecipient in carbonCopyRecipients)
				{
					if (string.IsNullOrWhiteSpace(carbonCopyRecipient))
					{
						continue;
					}

					message.CC.Add(new MailAddress(carbonCopyRecipient));
				}
			}

			message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

			if (attachments != null)
			{
				foreach (var data in attachments)
				{
					message.Attachments.Add(data);
				}
			}

			new Thread(() =>
			{
				Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
				SendEmail(message);
			}).Start();
		}

		public string GetBodyEmailFromObject(object data)
		{
			string response = string.Empty;

			if (data is null)
			{
				return response;
			}

			foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
			{
				try
				{
					response += $"{propertyInfo.Name}: {propertyInfo.GetValue(data)?.ToString()}\n";
				}
				catch (Exception ex)
				{
					Log.Error($"{nameof(EmailService)} - {nameof(GetBodyEmailFromObject)} - ERROR {ex.Message}");

					continue;
				}
			}

			response += "\n------------------------------\n\n";

			return response;
		}
	}
}