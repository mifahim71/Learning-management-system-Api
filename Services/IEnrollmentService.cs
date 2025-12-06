using LearningManagementSystemApi.Dtos;

namespace LearningManagementSystemApi.Services
{
    public interface IEnrollmentService
    {
        Task<bool> CourseEnrollmentAsync(int courseId, int studentId);
        Task<List<CourseResponseDto>> GetEnrolledCoursesAsync(int studentId);
        Task<bool> UpdateProgressAsync(int studentId, int courseId, EnrollmentProgressUpdateDto updateDto);
    }
}
