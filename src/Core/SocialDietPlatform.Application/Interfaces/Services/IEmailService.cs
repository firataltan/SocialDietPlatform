using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SocialDietPlatform.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string to, string firstName, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(string to, string resetToken, CancellationToken cancellationToken = default);
    Task SendAppointmentReminderAsync(string to, DateTime appointmentTime, CancellationToken cancellationToken = default);
}