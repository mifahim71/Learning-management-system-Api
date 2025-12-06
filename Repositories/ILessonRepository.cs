using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Repositories
{
    public interface ILessonRepository
    {
        Task<Lesson?> CreateLessonAsync(Lesson lesson);
        Task<List<Lesson>> GetLessonsByCourseIdAsync(int courseId);
        Task<Lesson?> GetLessonByIdAsync(int lessonId);

        Task<List<Lesson>> GetCustomerLessonAsync(int coursesId);
    }
}
