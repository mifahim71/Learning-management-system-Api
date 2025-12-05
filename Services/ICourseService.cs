using LearningManagementSystemApi.Dtos;

namespace LearningManagementSystemApi.Services
{
    public interface ICourseService
    {
        Task<CourseCreateResponseDto?> CreateCouseAsync(int userId, CourseCreateRequestDto requestDto);
        Task<List<CourseResponseDto>> GetAllCoursesAsync();
        Task<CourseWithLessonResponseDto> GetCourseByIdAsync(int courseId);

        Task<bool> UpdateCourseAsync(int courseId, int userId,  CustomerUpdateRequestDto requestDto);

        Task<bool> DeleteCourseAsync(int courseId);
    }
}
