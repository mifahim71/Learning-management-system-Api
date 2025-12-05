using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Repositories
{
    public interface IAuthRepository
    {
        Task<int> createAsync(AppUser user);
        Task<AppUser> validUserAsync(AuthLoginRequestDto requestDto);

        Task<AppUser?> GetByEmailAsync(string email);


    }
}
