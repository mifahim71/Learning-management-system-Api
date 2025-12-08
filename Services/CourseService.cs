using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;
using LearningManagementSystemApi.Exceptions;
using System.Text.Json;
using AutoMapper;

namespace LearningManagementSystemApi.Services
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;

        private static string REDIS_PREFIX = "redis:course:";

        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger, IRedisService redisService, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _logger = logger;
            _redisService = redisService;
            _mapper = mapper;
        }

        public async Task<CourseCreateResponseDto?> CreateCouseAsync(int userId, CourseCreateRequestDto requestDto)
        {

            var course = _mapper.Map<Course>(requestDto);


            course.AppUserId = userId;
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;
            
            var savedCourse = await _courseRepository.CreateAsync(course);

            if(savedCourse == null)
            {
                throw new CourseCreationFiledException("An error occurred while creating the course.");
            }

            await _redisService.RemoveAsync(REDIS_PREFIX + "all");

            return _mapper.Map<CourseCreateResponseDto>(savedCourse);
            
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


            var courseResponseDtos = _mapper.Map<List<CourseResponseDto>>(courses);

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

            var responseDto = _mapper.Map<CourseWithLessonResponseDto>(course);
            

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
