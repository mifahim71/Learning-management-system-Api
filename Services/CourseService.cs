using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;

namespace LearningManagementSystemApi.Services
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<CourseCreateResponseDto?> CreateCouseAsync(int userId, CourseCreateRequestDto requestDto)
        {
            var course = new Course
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var savedCourse = await _courseRepository.CreateAsync(course);

            if(savedCourse == null)
            {
                return null;
            }

            return new CourseCreateResponseDto
            {
                Id = savedCourse.Id,
                Title = savedCourse.Title,
                Description = savedCourse.Description
            };
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                return false;
            }

            await _courseRepository.DeleteAsync(course);
            return true;
        }

        public async Task<List<CourseResponseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();

            var courseResponseDtos = courses.Select(course =>
            {
                return new CourseResponseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    CreatedAt = course.CreatedAt,
                    InstructorId = course.AppUserId
                };
            }).ToList();

            return courseResponseDtos;
        }

        public async Task<CourseWithLessonResponseDto?> GetCourseByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);

            if (course == null)
            {
                return null;
            }

            var responseDto = new CourseWithLessonResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Lessons = course.Lessons?.Select(lesson => new LessonResponseDto
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    Content = lesson.Content
                }).ToList()
            };


            return responseDto;
        }

        public async Task<bool> UpdateCourseAsync(int courseId, int userId,  CustomerUpdateRequestDto requestDto)
        {
            
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                return false;
            }

            if(course.AppUserId != userId)
            {
                return false;
            }

            course.Title = requestDto.title ?? course.Title;
            course.Description = requestDto.description ?? course.Description;

            await _courseRepository.UpdateAsync(course);
            return true;
        }
    }
}
