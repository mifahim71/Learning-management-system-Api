namespace LearningManagementSystemApi.Dtos
{
    public class LessonCreateResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CourseId { get; set; }
    }
}
