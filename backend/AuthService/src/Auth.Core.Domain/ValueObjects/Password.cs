using Auth.Core.Domain.Exceptions;
using SharedKernel.Domain.ValueObjects;

namespace Auth.Core.Domain.ValueObjects
{
    public sealed class Password : ValueObject
    {
        public string Value { get; }

        // For Ef Core
        private Password() { }
        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new NullablePasswordException("Password cannot be empty or null");

            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Password password) => password.Value;
        public static explicit operator Password(string value) => new Password(value);
    }
}
