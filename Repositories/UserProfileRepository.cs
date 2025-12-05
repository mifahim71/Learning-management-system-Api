using LearningManagementSystemApi.Data;
using LearningManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystemApi.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {

        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserProfile?> GetByIdAsync(int id)
        {
            return await _context.UserProfiles.Include(u => u.AppUser).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(UserProfile userProfile)
        {
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();
        }
    }
}
