using LearningManagementSystemApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace LearningManagementSystemApi.Models
{
    public class AppUser
    {
        public int Id { get; set; }


        public string? Email { get; set; }


        public string? PasswordHash { get; set; }


        public UserRole Role { get; set; }


        public UserProfile? UserProfile { get; set; }
    }
}
