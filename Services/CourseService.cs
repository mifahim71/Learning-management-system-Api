using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;
using LearningManagementSystemApi.Exceptions;
using System.Text.Json;

namespace LearningManagementSystemApi.Services
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;
        private readonly IRedisService _redisService;

        private static string REDIS_PREFIX = "redis:course:";

        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger, IRedisService redisService)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _redisService = redisService;
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
                throw new CourseCreationFiledException("An error occurred while creating the course.");
            }

            await _redisService.RemoveAsync(REDIS_PREFIX + "all");
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
                throw new CourseDeletionFailedException($"Failed to delete the course wit Id:{courseId}");
            }
            
            await _courseRepository.DeleteAsync(course);
            await _redisService.RemoveAsync(REDIS_PREFIX + courseId);
            await _redisService.RemoveAsync(REDIS_PREFIX + "all");
            return true;
        }

        public async Task<List<CourseResponseDto>> GetAllCoursesAsync()
        {

            var cachedResponseDto = await _redisService.GetClassListAsync<CourseResponseDto>(REDIS_PREFIX + "all");
            if (cachedResponseDto != null) {
                return cachedResponseDto!;
            }
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

            await _redisService.SetClassListsAsync<CourseResponseDto>(REDIS_PREFIX + "all", courseResponseDtos, TimeSpan.FromMinutes(2));

            return courseResponseDtos;
        }

        public async Task<CourseWithLessonResponseDto> GetCourseByIdAsync(int courseId)
        {
            string? cachedResponse = await _redisService.GetAsync(REDIS_PREFIX + courseId);
            if (cachedResponse != null)
            {
                _logger.LogInformation("Taking data from Redis");
                return JsonSerializer.Deserialize<CourseWithLessonResponseDto>(cachedResponse)!;
            }


            var course = await _courseRepository.GetByIdAsync(courseId);

            if (course == null)
            {
                throw new CourseNotFoundException($"Course with Id:{courseId} not found");
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

            await _redisService.SetAsync(REDIS_PREFIX + courseId, JsonSerializer.Serialize(responseDto), TimeSpan.FromMinutes(2));


            return responseDto;
        }

        public async Task<bool> UpdateCourseAsync(int courseId, int userId,  CustomerUpdateRequestDto requestDto)
        {
            
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException($"Course with Id:{courseId} not found");
            }

            if(course.AppUserId != userId)
            {
                throw new ForbiddenException($"The userId:{userId} doesn't own this course");
            }

            course.Title = requestDto.title ?? course.Title;
            course.Description = requestDto.description ?? course.Description;

            await _courseRepository.UpdateAsync(course);
            await _redisService.RemoveAsync(REDIS_PREFIX + courseId);
            await _redisService.RemoveAsync(REDIS_PREFIX + "all");
            await _redisService.RemoveAsync(REDIS_PREFIX + courseId + ":lessons");
            return true;
        }
    }
}
