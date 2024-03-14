using $safeprojectname$.Modules.Email.Enums;
using System.Net.Mail;

namespace $safeprojectname$.Modules.Email.Interfaces.Services
{
	public interface IEmailService
	{
		void SendEmail(IEnumerable<string> recipients, string subject, string body, EmailBodyFormats bodyFormat, string? sender = null, string? displayName = null, IEnumerable<string>? carbonCopyRecipients = null, IEnumerable<Attachment>? attachments = null);
	}
}