using RoyalState.Core.Application.DTOs.Email;
using RoyalState.Core.Domain.Settings;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task SendAsync(EmailRequest request);
    }
}
