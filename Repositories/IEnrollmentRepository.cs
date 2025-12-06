using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<bool> CourseEnrollmentAsync(Enrollment enrollment);

        Task<Enrollment?> GetEnrollmentAsync(int studentId, int courseId);

        Task<List<Enrollment>> GetEnrollmentsAsync(int studentId);
        Task UpdateProgressAsync(Enrollment enrollment);
    }
}
