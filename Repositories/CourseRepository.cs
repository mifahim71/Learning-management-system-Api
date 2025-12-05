using LearningManagementSystemApi.Data;
using LearningManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystemApi.Repositories
{
    public class CourseRepository : ICourseRepository
    {

        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Course?> CreateAsync(Course course)
        {
            if (course == null)
            {
                return null;
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;

        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int courseId)
        {
            return await _context.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }
    }
}
