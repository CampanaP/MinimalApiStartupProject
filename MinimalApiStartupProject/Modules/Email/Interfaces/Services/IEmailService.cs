using $safeprojectname$.Modules.Email.Entities;

namespace $safeprojectname$.Modules.Email.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage message, CancellationToken cancellationToken = default);
    }
}