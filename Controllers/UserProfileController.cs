using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Exceptions;
using LearningManagementSystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {

        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserProfileResponseDto>> GetUserProfile()
        {

            string? email = User.Identity?.Name;

            UserProfileResponseDto responseDto = await _userProfileService.GetUserProfileAsync(email!);

            if (responseDto == null) {

                throw new UserNotFoundException("User Profile Not found");
            }

            return Ok(responseDto);
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<ActionResult> UpdateUserProfile([FromBody] UserProfileUpdateRequestDto requestDto)
        {
            
            string? email = User.Identity?.Name;

            var responseDto = await _userProfileService.UpdateUserProfileAsync(email!, requestDto);

            if (responseDto == null)
            {
                throw new UserNotFoundException("User profile not found");
            }

            return Ok(responseDto);
        }
    }
}
