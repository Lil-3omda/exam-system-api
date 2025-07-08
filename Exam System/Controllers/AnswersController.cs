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
    public class AnswersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AnswersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAnswers(int questionId)
        {
            var answers = await context.Answers
                .Where(a => a.QuestionId == questionId)
                .Select(a => new AnswerDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                })
                .ToListAsync();

            return Ok(answers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDto>> GetAnswer(int questionId, int id)
        {
            var answer = await context.Answers
                .Where(a => a.QuestionId == questionId && a.Id == id)
                .Select(a => new AnswerDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                })
                .FirstOrDefaultAsync();

            if (answer == null) return NotFound();

            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerDto>> CreateAnswer(int questionId, AnswerCreateDto dto)
        {
            var question = await context.Questions.FindAsync(questionId);
            if (question == null) return NotFound("Question not found");

            var answer = new Answer
            {
                Text = dto.Text,
                IsCorrect = dto.IsCorrect,
                QuestionId = questionId
            };

            context.Answers.Add(answer);
            await context.SaveChangesAsync();

            var result = new AnswerDto
            {
                Id = answer.Id,
                Text = answer.Text,
                IsCorrect = answer.IsCorrect
            };

            return CreatedAtAction(nameof(GetAnswer), new { questionId = questionId, id = answer.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int questionId, int id, AnswerCreateDto dto)
        {
            var answer = await context.Answers
                .FirstOrDefaultAsync(a => a.QuestionId == questionId && a.Id == id);

            if (answer == null) return NotFound();

            answer.Text = dto.Text;
            answer.IsCorrect = dto.IsCorrect;

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int questionId, int id)
        {
            var answer = await context.Answers
                .FirstOrDefaultAsync(a => a.QuestionId == questionId && a.Id == id);

            if (answer == null) return NotFound();

            context.Answers.Remove(answer);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
