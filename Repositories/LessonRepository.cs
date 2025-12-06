using LearningManagementSystemApi.Data;
using LearningManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystemApi.Repositories
{
    public class LessonRepository : ILessonRepository
    {

        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Lesson?> CreateLessonAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<List<Lesson>> GetCustomerLessonAsync(int coursesId)
        {
            var course = await _context.Courses.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.Id == coursesId);
            if (course == null)
            {
                return new List<Lesson>();
            }

            return course.Lessons;
        }

        public Task<Lesson?> GetLessonByIdAsync(int lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetLessonsByCourseIdAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}
