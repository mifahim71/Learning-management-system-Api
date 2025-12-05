using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;

namespace LearningManagementSystemApi.Services
{
    public class UserProfileService : IUserProfileService
    {

        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IAuthRepository _authRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository, IAuthRepository authRepository)
        {
            _userProfileRepository = userProfileRepository;
            _authRepository = authRepository;
        }
        public async Task<UserProfileResponseDto?> GetUserProfileAsync(string email)
        {

            var appUser = await _authRepository.GetByEmailAsync(email);

            

            if (appUser == null)
            {
                return null;
            }

            return new UserProfileResponseDto
            {
                FullName = appUser.UserProfile?.FullName,
                Bio = appUser.UserProfile?.Bio,
                Email = appUser.Email
            };

        }



        public async Task<UserProfileUpdateResponseDto?> UpdateUserProfileAsync(string email, UserProfileUpdateRequestDto requestDto)
        {
            var appUser = await _authRepository.GetByEmailAsync(email);

            if (appUser == null)
            {
                return null;
            }

            var userProfile = appUser.UserProfile;
            if(userProfile == null)
            {
                userProfile = new UserProfile
                {
                    AppUserId = appUser.Id,
                    FullName = requestDto.FullName,
                    Bio = requestDto.Bio
                };
                await _userProfileRepository.CreateAsync(userProfile);
            }
            else
            {
                userProfile.FullName = requestDto.FullName; 
                userProfile.Bio = requestDto.Bio;
                await _userProfileRepository.UpdateAsync(userProfile);    
            }

            return new UserProfileUpdateResponseDto
            {
                FullName = userProfile.FullName,
                Bio = userProfile.Bio
            };
        }

    }
}
