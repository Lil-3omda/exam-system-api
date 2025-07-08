namespace Exam_System.DTOs
{
    public class ExamCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string CreatedById { get; set; }
    }
}
