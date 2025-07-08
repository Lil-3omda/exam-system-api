using Exam_System.Data;
using Exam_System.DTOs;
using Exam_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentExamsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public StudentExamsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitExam([FromBody] SubmitExamDto dto)
        {
            var exam = await context.Exams
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == dto.ExamId);

            if (exam == null) return NotFound("Exam not found");

            var studentExam = new StudentExams
            {
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                TakenAt = DateTime.UtcNow
            };

            context.StudentExams.Add(studentExam);
            await context.SaveChangesAsync();

            int score = 0;

            foreach (var submitted in dto.Answers)
            {
                var question = exam.Questions.FirstOrDefault(q => q.Id == submitted.QuestionId);
                if (question == null) continue;

                var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == submitted.AnswerId);
                if (selectedAnswer == null) continue;

                bool isCorrect = selectedAnswer.IsCorrect;
                if (isCorrect) score++;

                var studentAnswer = new StudentAnswer
                {
                    StudentExamId = studentExam.Id,
                    QuestionId = submitted.QuestionId,
                    AnswerId = submitted.AnswerId,
                    IsCorrect = isCorrect
                };

                context.StudentAnswers.Add(studentAnswer);
            }

            studentExam.Score = score;
            await context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Exam submitted successfully",
                Score = score,
                TotalQuestions = exam.Questions.Count
            });
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentResults(string studentId)
        {
            var exams = await context.StudentExams
                .Where(se => se.StudentId == studentId)
                .Include(se => se.Exam)
                .ToListAsync();

            var result = exams.Select(se => new
            {
                ExamTitle = se.Exam.Title,
                Score = se.Score,
                TakenAt = se.TakenAt
            });

            return Ok(result);
        }
    }
}
