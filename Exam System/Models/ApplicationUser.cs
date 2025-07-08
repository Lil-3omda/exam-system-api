using Microsoft.AspNetCore.Identity;

namespace Exam_System.Models
{
    public class ApplicationUser:IdentityUser 
    {
        public ICollection<StudentExams> StudentExams { get; set; }
    }
}
