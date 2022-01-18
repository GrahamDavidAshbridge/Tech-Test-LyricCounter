namespace LyricCounter.Cli.Api
{
    public interface IApi
    {
        Task<T?> GetAsync<T>(string apiClientName, string uriExtension);
    }
}