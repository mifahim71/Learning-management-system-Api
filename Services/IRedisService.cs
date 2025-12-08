namespace LearningManagementSystemApi.Services
{
    public interface IRedisService
    {
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
        Task SetClassListsAsync<T>(string key, List<T> items, TimeSpan? expiry = null);
        Task<List<T?>?> GetClassListAsync<T>(string key);
    }
}
