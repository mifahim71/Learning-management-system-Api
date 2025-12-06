using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystemApi.Dtos
{
    public class EnrollmentProgressUpdateDto
    {
        [Required]
        [Range(0, 100)]
        public double Progress { get; set; }
    }
}
