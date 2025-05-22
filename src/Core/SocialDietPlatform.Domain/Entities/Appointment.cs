using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocialDietPlatform.Domain.Common;
using SocialDietPlatform.Domain.Enums;

namespace SocialDietPlatform.Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime ScheduledDateTime { get; set; }
    public int DurationMinutes { get; set; }
    public string? Notes { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public Guid ClientId { get; set; }
    public Guid DietitianId { get; set; }
    public string? MeetingUrl { get; set; }

    // Navigation Properties
    public virtual User Client { get; set; } = null!;
    public virtual User Dietitian { get; set; } = null!;

    public DateTime EndDateTime => ScheduledDateTime.AddMinutes(DurationMinutes);
    public bool IsUpcoming => ScheduledDateTime > DateTime.UtcNow && Status == AppointmentStatus.Scheduled;
}