using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialDietPlatform.Domain.Enums;

public enum NotificationType
{
    Like = 1,
    Comment = 2,
    Follow = 3,
    DietPlanAssigned = 4,
    AppointmentReminder = 5,
    RecipeShared = 6
}