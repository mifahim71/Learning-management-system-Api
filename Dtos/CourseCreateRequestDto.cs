using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystemApi.Dtos
{
    public class CourseCreateRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
