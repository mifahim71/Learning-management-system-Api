namespace LearningManagementSystemApi.Dtos
{
    public class CourseWithLessonResponseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<LessonResponseDto>? Lessons { get; set; }
    }
}
