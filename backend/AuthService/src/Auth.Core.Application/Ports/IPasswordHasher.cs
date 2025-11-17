namespace Auth.Core.Application.Ports
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string password, string hashedPassword);
    }
}
