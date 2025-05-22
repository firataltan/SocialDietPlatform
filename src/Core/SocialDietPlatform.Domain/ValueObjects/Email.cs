using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace SocialDietPlatform.Domain.ValueObjects;

public record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty");

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format");

        Value = value.ToLowerInvariant();
    }

    private static bool IsValidEmail(string email)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return emailRegex.IsMatch(email);
    }

    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string email) => new(email);
}
