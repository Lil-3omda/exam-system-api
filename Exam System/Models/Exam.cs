namespace Exam_System.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<StudentExams> StudentExams { get; set; }
    }

}
