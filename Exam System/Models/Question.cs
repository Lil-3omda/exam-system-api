namespace Exam_System.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; } 

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }

}
