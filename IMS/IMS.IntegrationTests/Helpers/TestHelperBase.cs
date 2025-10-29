namespace IMS.IntegrationTests.Helpers;

public abstract class TestHelperBase(CustomWebApplicationFactory factory) 
    : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory = factory;
    protected readonly HttpClient Client = factory.CreateClient();

    public Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ImsDbContext>();

        dbContext.Database.EnsureDeletedAsync();
        dbContext.Database.EnsureCreatedAsync();

        return Task.CompletedTask;
    }
    public Task DisposeAsync() => Task.CompletedTask;

    protected async Task<TEntity> AddEntityAsync<TEntity>(TEntity entity) 
        where TEntity : EntityBase
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ImsDbContext>();
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    protected async Task<List<TEntity>> AddEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) 
        where TEntity : EntityBase
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ImsDbContext>();
        dbContext.AddRange(entities);
        await dbContext.SaveChangesAsync();
        return [.. entities];
    }

    protected IServiceScope CreateScope() => _factory.Services.CreateScope();

    protected static T? Deserialize<T>(string content) => JsonConvert.DeserializeObject<T>(content);
}
