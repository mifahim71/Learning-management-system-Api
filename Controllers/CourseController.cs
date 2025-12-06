using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly ICourseService _courseService;
        private readonly ILessonService _lessonService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger, ILessonService lessonService)
        {
            _courseService = courseService;
            _logger = logger;
            _lessonService = lessonService;
        }

        [HttpPost]
        [Authorize(Roles = "INSTRUCTOR,ADMIN")]
        public async Task<ActionResult<CourseCreateResponseDto>> CreateCourse([FromBody] CourseCreateRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var responseDto = await _courseService.CreateCouseAsync(userId, requestDto);
            if (responseDto == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the course.");
            }

            return Ok(responseDto);
        }


        [HttpGet]
        public async Task<ActionResult<List<CourseResponseDto>>> GetAllCoursesAsync()
        {

            var responseDtos = await _courseService.GetAllCoursesAsync();
            return Ok(responseDtos);

        }


        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseWithLessonResponseDto>> GetCourseByIdAsync(int courseId)
        {
            var responseDto = await _courseService.GetCourseByIdAsync(courseId);

            if (responseDto == null)
            {
                return NotFound();
            }

            return Ok(responseDto);
        }


        [HttpPut("{courseId}")]
        [Authorize(Roles = "INSTRUCTOR,ADMIN")]
        public async Task<IActionResult> UpdateCourseAsync(int courseId, [FromBody] CustomerUpdateRequestDto requestDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            bool success = await _courseService.UpdateCourseAsync(courseId, userId, requestDto);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }


        [HttpDelete("{courseId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCourseAsync(int courseId)
        {
            bool success = await _courseService.DeleteCourseAsync(courseId);
            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("{courseId}/lessons")]
        [Authorize(Roles = "INSTRUCTOR,ADMIN")]
        public async Task<ActionResult<LessonCreateResponseDto>> AddLessonAsync(int courseId, [FromBody] LessonCreateRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int instructorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var lessonCreateResponseDto = await _lessonService.CreateLessonAsync(instructorId, courseId, requestDto);

            if (lessonCreateResponseDto == null)
            {
                return BadRequest();
            }

            return Ok(lessonCreateResponseDto);
        }

        [HttpGet("{courseId}/lessons")]
        [Authorize]
        public async Task<List<LessonResponseDto>> GetLessonsAsync(int courseId)
        {
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return await _lessonService.GetCourseLessonsAsync(role, userId, courseId);
        }
    }
}
