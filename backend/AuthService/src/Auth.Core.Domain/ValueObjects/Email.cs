using Auth.Core.Domain.Exceptions;
using SharedKernel.Domain.Regexs;
using SharedKernel.Domain.ValueObjects;

namespace Auth.Core.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public string Value { get; }

        // For Ef Core
        private Email() { }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new NullableEmailException("Email cannot be empty or null");

            value = value.Trim().ToLowerInvariant();

            if (!RegexPatterns.Email.IsMatch(value))
                throw new InvalidEmailFormatException("Email is not in a valid format.");

            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
        public static explicit operator Email(string value) => new Email(value);
    }
}
