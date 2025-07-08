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
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public QuestionsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions(int examId)
        {
            var questions = await context.Questions
             .Where(q => q.ExamId == examId)
             .Select(q => new QuestionDto
             {
                 Id = q.Id,
                 Text = q.Text,
                 Type = q.Type
             })
             .ToListAsync();

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int examId, int id)
        {
            var question = await context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id && q.ExamId == examId);

            if (question == null) return NotFound();

            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion(int examId, QuestionCreateDto dto)
        {
            var exam = await context.Exams.FindAsync(examId);
            if (exam == null) return NotFound("Exam not found");

            var question = new Question
            {
                Text = dto.Text,
                Type = dto.Type,
                ExamId = examId
            };

            context.Questions.Add(question);
            await context.SaveChangesAsync();

            var resultDto = new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Type = question.Type
            };

            return CreatedAtAction(nameof(GetQuestion), new { examId = examId, id = question.Id }, resultDto);
        }
        

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int examId, int id, QuestionCreateDto dto)
        {
            var question = await context.Questions
                .FirstOrDefaultAsync(q => q.Id == id && q.ExamId == examId);

            if (question == null) return NotFound();

            question.Text = dto.Text;
            question.Type = dto.Type;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int examId, int id)
        {
            var question = await context.Questions
                .FirstOrDefaultAsync(q => q.Id == id && q.ExamId == examId);

            if (question == null) return NotFound();

            context.Questions.Remove(question);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
