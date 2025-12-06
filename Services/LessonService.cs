using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Repositories;
using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Services
{
    public class LessonService : ILessonService
    {

        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;

        public LessonService(ILessonRepository lessonRepository, ICourseRepository _couseRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = _couseRepository;
        }

        public async Task<LessonCreateResponseDto?> CreateLessonAsync(int instructorId, int courseId, LessonCreateRequestDto requestDto)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null || course.AppUserId != instructorId)
            {
                return null; 
            }

            var lesson = new Lesson
            {
                Title = requestDto.Title,
                Content = requestDto.Content,
                CourseId = courseId
            };

            var savedLesson = await _lessonRepository.CreateLessonAsync(lesson);
            if (savedLesson == null)
            {
                return null;
            }

            return new LessonCreateResponseDto
            {
                Id = savedLesson.Id,
                Title = savedLesson.Title!,
                Content = savedLesson.Content!,
                CourseId = savedLesson.CourseId
            };  
        }

        public async Task<List<LessonResponseDto>> GetCourseLessonsAsync(string role, int userId, int courseId)
        {
            if(role == "ADMIN" || role == "INSTRUCTOR")
            {
                var course = await _courseRepository.GetByIdAsync(courseId);
                if (course == null || course.AppUserId != userId)
                {
                    return new List<LessonResponseDto>();
                }

                if(course.Lessons == null)
                {
                    return new List<LessonResponseDto>();
                }

                return course.Lessons.Select(lesson => new LessonResponseDto
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    Content = lesson.Content
                }).ToList();
            }
            
            return new List<LessonResponseDto>();

        }

        public Task<List<LessonResponseDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
