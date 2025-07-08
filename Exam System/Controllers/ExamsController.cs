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
    public class ExamsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ExamsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
        {
            return await context.Exams.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> GetExam(int id)
        {
            var exam = await context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null)
                return NotFound();

            return exam;
        }

        [HttpPost]
        public async Task<ActionResult<Exam>> CreateExam(ExamCreateDto dto)
        {
            var exam = new Exam
            {
                Title = dto.Title,
                Description = dto.Description,
                Duration = dto.Duration,
                CreatedById = dto.CreatedById,
                CreatedAt = DateTime.UtcNow
            };

            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExam), new { id = exam.Id }, exam);
        }

        // change Exam exam with ExamEditDto
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam(int id, Exam exam)
        {
            if (id != exam.Id) return BadRequest();

            context.Entry(exam).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var exam = await context.Exams.FindAsync(id);
            if (exam == null) return NotFound();

            context.Exams.Remove(exam);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
