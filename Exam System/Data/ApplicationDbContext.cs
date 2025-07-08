using Exam_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Exam_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<StudentExams> StudentExams { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.CreatedBy)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentExams>()
                .HasOne(se => se.Student)
                .WithMany(u => u.StudentExams)
                .HasForeignKey(se => se.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.StudentExams)
                .WithMany(se => se.StudentAnswers)
                .HasForeignKey(sa => sa.StudentExamId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Answer)
                .WithMany(a => a.StudentAnswers)
                .HasForeignKey(sa => sa.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
