using MinimalApiStartupProject.Modules.Email.Entities;

namespace MinimalApiStartupProject.Modules.Email.Interfaces.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Async method to send EmailMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}