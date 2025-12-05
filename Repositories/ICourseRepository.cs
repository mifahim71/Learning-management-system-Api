using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Repositories
{
    public interface ICourseRepository
    {
        Task<Course> CreateAsync(Course course);

        Task<List<Course>> GetAllAsync();

        Task<Course> GetByIdAsync(int courseId);

        Task UpdateAsync(Course course);

        Task DeleteAsync(Course course);
    }
}
