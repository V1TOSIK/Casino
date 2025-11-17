using Auth.Core.Domain.Enums;
using Auth.Core.Domain.Exceptions;
using Auth.Core.Domain.ValueObjects;
using SharedKernel.Domain.AggregateRoot;

namespace Auth.Core.Domain.Entities
{
    public class UserEntity : AggregateRoot<Guid>
    {
        private UserEntity() { }
        private UserEntity(
            Email? email,
            PhoneNumber? phoneNumber,
            Password? password,
            Guid roleId)
        {
            if (email is null && phoneNumber is null)
                throw new MissingAuthCredentialException();

            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            RoleId = roleId;
            RegistrationDate = DateTime.UtcNow;
        }

        public Email? Email { get; private set; }
        public PhoneNumber? PhoneNumber { get; private set; }
        public Password? Password { get; private set; }
        public Guid RoleId { get; private set; }
        public DateTime RegistrationDate { get; }
        public bool IsBanned { get; private set; } = false;
        public string? BanReason { get; private set; } = null;
        public DateTime? BannedAt { get; private set; } = null;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }

        private readonly List<ExternalLogin> _externalLogins = [];
        public IReadOnlyCollection<ExternalLogin> ExternalLogins => _externalLogins.AsReadOnly();

        public static UserEntity Create(string? emailValue, string? phoneNumberValue, string? passwordValue, Guid roleId)
        {
            var email = string.IsNullOrWhiteSpace(emailValue) ? null : new Email(emailValue);
            var phoneNumber = string.IsNullOrWhiteSpace(phoneNumberValue) ? null : new PhoneNumber(phoneNumberValue);
            var password = string.IsNullOrWhiteSpace(passwordValue) ? null : new Password(passwordValue);

            if (roleId == Guid.Empty) throw new EmptyRoleIdException("RoleId cannot be empty.");

            return new UserEntity(
                email,
                phoneNumber,
                password,
                roleId);
        }

        public void AddExternalLogin(string providerUserId, string providerValue)
        {
            if (!Enum.TryParse<AuthProvider>(providerValue, true, out var provider))
                throw new InvalidProviderException($"Invalid provider: {provider}");
            if (_externalLogins.Any(e => e.Provider == provider && e.ProviderUserId == providerUserId))
                throw new ExternalLoginAlreadyExistException("External login already exists");

            _externalLogins.Add(ExternalLogin.Create(Id, provider, providerUserId));
        }

        public void MarkAsDeleted()
        {
            if (IsDeleted)
                throw new UserAlreadyDeletedException("User is already deleted.");

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            if (!IsDeleted)
                throw new UserNotDeletedException("User is not deleted.");

            IsDeleted = false;
            DeletedAt = null;
        }

        public void Ban(string reason)
        {
            if (IsBanned)
                throw new UserAlreadyBannedException("User is already banned.");
            IsBanned = true;
            BanReason = reason;
            BannedAt = DateTime.UtcNow;
        }

        public void Unban()
        {
            if (!IsBanned)
                throw new UserNotBannedException("User is not banned.");
            IsBanned = false;
            BanReason = null;
            BannedAt = null;
        }

        public void SetEmail(string emailValue)
        {
            EnsureActive();
            if (string.IsNullOrWhiteSpace(emailValue))
                throw new InvalidEmailFormatException("Email cannot be empty or null.");
            if (Email != null)
                throw new EmailAlreadySetException("Email is already set.");
            Email = new Email(emailValue);
        }

        public void SetPhone(string phoneValue)
        {
            EnsureActive();
            if (string.IsNullOrWhiteSpace(phoneValue))
                throw new InvalidPhoneNumberFormatException("Phone number cannot be empty or null.");
            if (PhoneNumber != null)
                throw new PhoneNumberAlreadySetException("Phone number is already set.");
            PhoneNumber = new PhoneNumber(phoneValue);
        }

        public void SetPassword(string passwordValue)
        {
            EnsureActive();
            if (string.IsNullOrWhiteSpace(passwordValue))
                throw new NullablePasswordException("Password cannot be empty or null.");
            if (Password != null)
                throw new PasswordAlreadySetException("Password is already set. Use UpdatePassword method to change it.");
            Password = new Password(passwordValue); ;
        }

        public void UpdatePassword(string passwordValue)
        {
            EnsureActive();
            if (string.IsNullOrWhiteSpace(passwordValue))
                throw new NullablePasswordException("Password cannot be empty or null.");
            if (Password == null)
                throw new UserRegisteredByProviderException("User is registered by OAuth");
            Password = new Password(passwordValue);
        }

        public void UpdateRole(Guid roleId) => RoleId = roleId;

        public void EnsureActive()
        {
            if (IsDeleted)
                throw new UserAlreadyDeletedException("User is deleted.");
            if (IsBanned)
                throw new UserAlreadyBannedException("User is baned.");
        }
    }
}
