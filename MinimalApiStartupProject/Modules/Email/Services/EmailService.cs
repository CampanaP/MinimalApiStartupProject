using MailKit.Net.Smtp;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MinimalApiStartupProject.Infrastructures.Attributes;
using MinimalApiStartupProject.Infrastructures.StringExtensions;
using MinimalApiStartupProject.Modules.Email.Entities;
using MinimalApiStartupProject.Modules.Email.Enums;
using MinimalApiStartupProject.Modules.Email.Interfaces.Services;
using MinimalApiStartupProject.Modules.Email.Settings;
using System.Text.Json;

namespace MinimalApiStartupProject.Modules.Email.Services
{
    [ScopedLifetime]
    internal class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Private method to get MimeMessage object from EmailMessage
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private MimeMessage? GetMimeMessage(EmailMessage message)
        {
            MimeMessage? mimeMessage = null;

            EmailSetting? setting = _configuration.Get<EmailSetting>();
            if (setting is null || setting.SenderAddress is null)
            {
                return mimeMessage;
            }

            if (setting.SenderDisplayName is null && string.IsNullOrWhiteSpace(message.SenderDisplayName))
            {
                message.SenderDisplayName = setting.SenderAddress.Split("@")?[0] ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(message.SenderAddress))
                {
                    message.SenderDisplayName = message.SenderAddress.Split("@")?[0] ?? string.Empty;
                }
            }

            MailboxAddress sender = new MailboxAddress(setting.SenderDisplayName, setting.SenderAddress);

            if (!string.IsNullOrWhiteSpace(message.SenderAddress))
            {
                sender.Address = message.SenderAddress;
            }

            if (!string.IsNullOrWhiteSpace(message.SenderDisplayName))
            {
                sender.Name = message.SenderDisplayName;
            }

            List<MailboxAddress> recipientMailboxAddresses = new List<MailboxAddress>();
            if (!message.Recipients.IsNullOrEmpty())
            {
                foreach (string recipientEmail in message.Recipients!)
                {
                    recipientMailboxAddresses.Add(new MailboxAddress(recipientEmail.Split("@")?[0] ?? string.Empty, recipientEmail));
                }
            }

            BodyBuilder bodyBuilder = new BodyBuilder()
            {
                HtmlBody = message.BodyFormat == EmailBodyFormat.Html ? message.Body : null,
                TextBody = message.BodyFormat == EmailBodyFormat.Text ? message.Body : null,
            };

            if (!message.Attachments.IsNullOrEmpty())
            {
                foreach (System.Net.Mail.Attachment attachment in message.Attachments!)
                {
                    bodyBuilder.Attachments.Add(new MimePart()
                    {
                        Content = new MimeContent(attachment.ContentStream),
                        FileName = attachment.Name
                    });
                }
            }

            mimeMessage = new MimeMessage(sender, recipientMailboxAddresses, message.Subject, bodyBuilder.ToMessageBody());

            if (!message.CarbonCopyRecipients.IsNullOrEmpty())
            {
                foreach (string carbonCopyRecipient in message.CarbonCopyRecipients!)
                {
                    mimeMessage.Cc.Add(new MailboxAddress(carbonCopyRecipient.Split("@")?[0] ?? string.Empty, carbonCopyRecipient));
                }
            }

            if (!message.BlindCarbonCopyRecipients.IsNullOrEmpty())
            {
                foreach (string blindCarbonCopyRecipient in message.BlindCarbonCopyRecipients!)
                {
                    mimeMessage.Bcc.Add(new MailboxAddress(blindCarbonCopyRecipient.Split("@")?[0] ?? string.Empty, blindCarbonCopyRecipient));
                }
            }

            return mimeMessage;
        }

        /// <summary>
        /// Private async method to send MimeMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
        {
            EmailSetting? settings = _configuration.Get<EmailSetting>();

            if (settings is null || string.IsNullOrWhiteSpace(settings.Host) || string.IsNullOrWhiteSpace(settings.Username) || string.IsNullOrWhiteSpace(settings.Password))
            {
                _logger.LogException(nameof(EmailService), nameof(SendEmailAsync), JsonSerializer.Serialize(settings), "Configurations are not valid");

                throw new Exception("Send email - Configurations are not valid");
            }

            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync(settings.Host, 587, true, cancellationToken);

                    await smtpClient.AuthenticateAsync(settings.Username, settings.Password, cancellationToken);

                    await smtpClient.SendAsync(message, cancellationToken);

                    await smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(nameof(EmailService), nameof(SendEmailAsync), JsonSerializer.Serialize(message), ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Async method to send EmailMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            MimeMessage? mimeMessage = GetMimeMessage(message);
            if (mimeMessage is null)
            {
                _logger.LogException(nameof(EmailService), nameof(SendEmailAsync), JsonSerializer.Serialize(message), "MimeMessage is null");

                throw new Exception("MimeMessage is null");
            }

            await SendAsync(mimeMessage, cancellationToken);
        }
    }
}