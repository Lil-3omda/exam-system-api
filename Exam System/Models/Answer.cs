namespace Exam_System.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }

}
