namespace Exam_System.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }

        public int StudentExamId { get; set; }
        public StudentExams StudentExams { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public bool IsCorrect { get; set; }
    }

}
