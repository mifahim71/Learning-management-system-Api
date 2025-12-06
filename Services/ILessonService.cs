using LearningManagementSystemApi.Dtos;

namespace LearningManagementSystemApi.Services
{
    public interface ILessonService
    {
        Task<LessonCreateResponseDto?> CreateLessonAsync(int instructorId, int courseId, LessonCreateRequestDto requestDto);
        Task<List<LessonResponseDto>> GetLessonsByCourseIdAsync(int courseId);

        Task<List<LessonResponseDto>> GetCourseLessonsAsync(string role, int userId,  int courseId);
    }
}
