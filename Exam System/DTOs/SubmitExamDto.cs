namespace Exam_System.DTOs
{
    public class SubmitExamDto
    {
        public int ExamId { get; set; }
        public string StudentId { get; set; }
        public List<SubmitAnswerDto> Answers { get; set; }
    }
}
