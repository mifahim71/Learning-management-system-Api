using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystemApi.Dtos
{
    public class CustomerUpdateRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? title { get; set; }

        [Required]
        public string? description { get; set; }
    }
}
