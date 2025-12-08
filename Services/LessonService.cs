using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Repositories;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Exceptions;
using AutoMapper;

namespace LearningManagementSystemApi.Services
{
    public class LessonService : ILessonService
    {

        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        

        private const string REDIS_PREFIX = "redis:course:";

        public LessonService(ILessonRepository lessonRepository,
                             ICourseRepository _couseRepository, 
                             IEnrollmentRepository enrollmentRepository,
                             IRedisService redisService,
                             IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = _couseRepository;
            _enrollmentRepository = enrollmentRepository;
            _redisService = redisService;
            _mapper = mapper;
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

            var lesson = _mapper.Map<Lesson>(requestDto);
            lesson.CourseId = courseId;

            var savedLesson = await _lessonRepository.CreateLessonAsync(lesson);
            if (savedLesson == null)
            {
                throw new LessonCreationFailedException("Failed to create a new Lesson");
            }

            await _redisService.RemoveAsync(REDIS_PREFIX + courseId + ":lessons");

            return _mapper.Map<LessonCreateResponseDto>(savedLesson);

        }

        public async Task<List<LessonResponseDto>> GetCourseLessonsAsync(string role, int userId, int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new CourseNotFoundException($"There is no course exists with the courseId:{courseId}");

            if(course.Lessons == null)
                return new List<LessonResponseDto>();

            var cachedResponseDtos = await _redisService.GetClassListAsync<LessonResponseDto>(REDIS_PREFIX + courseId + ":lessons");
            if(cachedResponseDtos != null)
            {
                return cachedResponseDtos!;
            }

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

            var lessonResponseDtos = _mapper.Map<List<LessonResponseDto>>(course.Lessons);
            

            await _redisService.SetClassListsAsync<LessonResponseDto>(REDIS_PREFIX + courseId + ":lessons", lessonResponseDtos, TimeSpan.FromMinutes(1));

            return lessonResponseDtos;
        }


        public Task<List<LessonResponseDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
