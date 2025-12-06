using LearningManagementSystemApi.Data;
using LearningManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystemApi.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {

        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<bool> CourseEnrollmentAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Enrollment?> GetEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.AppUserId ==  studentId && e.CourseId == courseId);
            if (enrollment == null)
            {
                return null;
            }

            return enrollment;
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync(int studentId)
        {
           return await _context.Enrollments.Include(e => e.Course).Where(enrollment => enrollment.AppUserId == studentId).ToListAsync();
        }

        public async Task UpdateProgressAsync(Enrollment enrollment)
        {
            _context.Update(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
