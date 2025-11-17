namespace SharedKernel.DbInitializer
{
    public interface IDbInitializer
    {
        Task InitializeAsync(IServiceProvider serviceProvider);
    }
}
