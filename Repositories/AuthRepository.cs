using LearningManagementSystemApi.Data;
using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystemApi.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<int> createAsync(AppUser user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser> validUserAsync(AuthLoginRequestDto requestDto)
        {

            var appUser = await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Email == requestDto.Email);
            if (appUser == null) {
                return null;
            }

            var result = new PasswordHasher<AppUser>().VerifyHashedPassword(appUser, appUser.PasswordHash!, requestDto.Password!);
            if(result == PasswordVerificationResult.Failed) {
                return null;
            }

            return appUser;
        }
    }
}
