using System.Text.RegularExpressions;

namespace SharedKernel.Domain.Regexs
{
    public static class RegexPatterns
    {
        public static readonly Regex Email = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex Phone = new(
            @"^\+?\d{10,15}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmailOrPhone(this string value)
        {
            return RegexPatterns.Email.IsMatch(value) ||
                   RegexPatterns.Phone.IsMatch(value);
        }
    }
}