using LearningManagementSystemApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystemApi.Dtos
{
    public class AuthRegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 50 characters.")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Bio { get; set; }

        }
}
