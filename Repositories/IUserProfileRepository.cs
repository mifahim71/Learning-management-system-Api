using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Repositories
{
    public interface IUserProfileRepository
    {
        Task<bool> CreateAsync(UserProfile userProfile);

        Task<UserProfile?> GetByIdAsync(int id);
        Task UpdateAsync(UserProfile userProfile);
    }
}
