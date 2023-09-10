using $safeprojectname$.Modules.Email.Enums;
using System.Net.Mail;

namespace $safeprojectname$.Modules.Email.Interfaces.Services
{
	public interface IEmailService
	{
		string GetBodyEmailFromObject(object data);

		void SendEmail(IEnumerable<string> recipients, string @object, string body, EmailBodyFormats bodyFormat, List<Attachment>? attachments = null, string? sender = null, string? displayName = null, IEnumerable<string>? carbonCopyRecipients = null);

		void SendEmail(IEnumerable<string> recipients, string @object, string body, EmailBodyFormats bodyFormat, string? sender = null, string? displayName = null, IEnumerable<string>? carbonCopyRecipients = null);
	}
}