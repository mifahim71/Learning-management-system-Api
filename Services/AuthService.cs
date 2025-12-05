using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LearningManagementSystemApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public AuthService(IAuthRepository authRepository, IUserProfileRepository userProfileRepository)
        {
            _authRepository = authRepository;
            _userProfileRepository = userProfileRepository;
        }
        
        public async Task<AuthRegisterResponseDto> Register(AuthRegisterRequestDto requestDto)
        {
            
            var user = new AppUser
            {
                Email = requestDto.Email,
                Role = requestDto.Role,  
            };
            user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(user, requestDto.Password);

            int userId = await _authRepository.createAsync(user);


            bool isCreated = await _userProfileRepository.CreateAsync(new UserProfile
            {
                AppUserId = userId,
                FullName = requestDto.FullName,
                Bio = requestDto.Bio,
            });

            if (!isCreated)
            {
                throw new Exception("Failed to create user profile");
            }

            return new AuthRegisterResponseDto
            {
                Email = user.Email,
                Role = user.Role.ToString(),
            };


            
        }

        public async Task<AppUser> ValidateUser(AuthLoginRequestDto requestDto)
        {
            return await _authRepository.validUserAsync(requestDto);
        }
    }
}
