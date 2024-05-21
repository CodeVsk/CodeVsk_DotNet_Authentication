namespace CodeVsk.Dotnet.Authentication.Domain.Contracts.Providers
{
    public interface IRedisProvider
    {
        Task<string> GetAsync(string key, CancellationToken cancellationToken);
        Task<string> SetAsync(string key, string value, CancellationToken cancellationToken);
    }
}
