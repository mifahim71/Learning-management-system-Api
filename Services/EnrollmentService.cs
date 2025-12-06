
using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using LearningManagementSystemApi.Repositories;

namespace LearningManagementSystemApi.Services
{
    public class EnrollmentService : IEnrollmentService
    {

        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
        }
        public async Task<bool> CourseEnrollmentAsync(int courseId, int studentId)
        {

            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
            {
                return false;
            }

            var enrollment = await _enrollmentRepository.GetEnrollmentAsync(studentId, courseId);
            if (enrollment != null) {
                return false;
            }

            var savedEnrollment = new Enrollment
            {
                AppUserId = studentId,
                CourseId = courseId,
                Progress = 0,
                EnrolledAt = DateTime.UtcNow
            };

            bool success = await _enrollmentRepository.CourseEnrollmentAsync(savedEnrollment);
            return success;
        }

        public async Task<List<CourseResponseDto>> GetEnrolledCoursesAsync(int studentId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsAsync(studentId);

            var responseDto = enrollments.Select(e => new CourseResponseDto
            {
                Title = e.Course!.Title,
                Description = e.Course!.Description,
                Id = e.CourseId,
                CreatedAt = e.Course.CreatedAt,
                InstructorId = studentId
            }).ToList();

            return responseDto;
        }

        public async Task<bool> UpdateProgressAsync(int studentId, int courseId, EnrollmentProgressUpdateDto updateDto)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentAsync(studentId, courseId);
            if (enrollment == null)
            {
                return false;
            }

            enrollment.Progress = updateDto.Progress;

            await _enrollmentRepository.UpdateProgressAsync(enrollment);
            return true;

        }
    }
}
