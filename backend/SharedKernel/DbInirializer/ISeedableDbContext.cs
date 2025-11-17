namespace SharedKernel.DbInitializer
{
    public interface ISeedableDbContext
    {
        Task SeedAsync();
    }
}
