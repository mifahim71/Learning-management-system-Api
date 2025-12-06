namespace LearningManagementSystemApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }


        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }


        public List<Lesson>? Lessons { get; set; } = new();

        public List<Enrollment>? Enrollments { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
