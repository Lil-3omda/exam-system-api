namespace Exam_System.Models
{
    public class StudentExams
    {
        public int Id { get; set; }

        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public float Score { get; set; }
        public DateTime TakenAt { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; }
    }

}
