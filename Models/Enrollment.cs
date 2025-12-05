namespace LearningManagementSystemApi.Models
{
    public class Enrollment
    {

        public int Id { get; set; }

        public int AppUserId { get; set; }
        public AppUser? AppUser { get; set; }


        public int CourseId { get; set; }
        public Course? Course { get; set; }


        public DateTime EnrolledAt { get; set; }
        public double Progress { get; set; }
    }
}
