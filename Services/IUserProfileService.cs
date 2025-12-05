using LearningManagementSystemApi.Dtos;

namespace LearningManagementSystemApi.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileResponseDto> GetUserProfileAsync(string email);
        Task<UserProfileUpdateResponseDto?> UpdateUserProfileAsync(string email, UserProfileUpdateRequestDto requestDto);
    }
}
