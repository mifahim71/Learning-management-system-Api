using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystemApi.Dtos
{
    public class UserProfileUpdateRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string? Bio { get; set; }
    }
}
