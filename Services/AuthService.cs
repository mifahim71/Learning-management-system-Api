using AutoMapper;
using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Exceptions;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LearningManagementSystemApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }
        
        public async Task<AuthRegisterResponseDto> Register(AuthRegisterRequestDto requestDto)
        {

            var user = _mapper.Map<AppUser>(requestDto);
            
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
                throw new ProfileCreationFailedException("Failed to create user profile");
            }

            return _mapper.Map<AuthRegisterResponseDto>(user);
        }

        public async Task<AppUser> ValidateUser(AuthLoginRequestDto requestDto)
        {
            return await _authRepository.validUserAsync(requestDto);
        }
    }
}
