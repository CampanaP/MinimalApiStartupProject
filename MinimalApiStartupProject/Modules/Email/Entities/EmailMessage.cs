using System.Net.Mail;
using MinimalApiStartupProject.Modules.Email.Enums;

namespace MinimalApiStartupProject.Modules.Email.Entities
{
    public class EmailMessage
    {
        public string? SenderAddress { get; set; }

        public string? SenderDisplayName { get; set; }

        public IEnumerable<string>? Recipients { get; set; }

        public IEnumerable<string>? CarbonCopyRecipients { get; set; }

        public IEnumerable<string>? BlindCarbonCopyRecipients { get; set; }

        public IEnumerable<Attachment>? Attachments { get; set; }

        public required string Subject { get; set; }

        public required string Body { get; set; }

        public EmailBodyFormat BodyFormat { get; set; }
    }
}