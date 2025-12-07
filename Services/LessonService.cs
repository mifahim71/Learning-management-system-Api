using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Repositories;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Exceptions;

namespace LearningManagementSystemApi.Services
{
    public class LessonService : ILessonService
    {

        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public LessonService(ILessonRepository lessonRepository, ICourseRepository _couseRepository, IEnrollmentRepository enrollmentRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = _couseRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<LessonCreateResponseDto?> CreateLessonAsync(int instructorId, int courseId, LessonCreateRequestDto requestDto)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with courseId:{courseId} not found"); 
            }

            if(course.AppUserId != instructorId)
            {
                throw new ForbiddenException($"The instructorId:{instructorId}, doesn't own this course");
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
                throw new LessonCreationFailedException("Failed to create a new Lesson");
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
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new CourseNotFoundException($"There is no course exists with the courseId:{courseId}");

            if(course.Lessons == null)
                return new List<LessonResponseDto>();


            if (role == "ADMIN" || role == "INSTRUCTOR")
            {
                if (course.AppUserId != userId)
                    throw new ForbiddenException($"The instructor doesn't own this course:{courseId}");
            }
            else if (role == "STUDENT")
            {
                var enrollment = await _enrollmentRepository.GetEnrollmentAsync(userId, courseId);
                if (enrollment == null)
                    throw new ForbiddenException("You aren't enrolled in this course");
            }
            else
            {
                throw new ForbiddenException("Invalid user credentials");
            }

            return course.Lessons
                .Select(lesson => new LessonResponseDto
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    Content = lesson.Content
                })
                .ToList();
        }


        public Task<List<LessonResponseDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
