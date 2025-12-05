namespace LearningManagementSystemApi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }


        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
