using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using System.Security.Claims;

namespace LearningManagementSystemApi.Services
{
    public interface IAuthService
    {
        Task<AuthRegisterResponseDto> Register(AuthRegisterRequestDto requestDto);

        Task<AppUser> ValidateUser(AuthLoginRequestDto requestDto);

    }
}
