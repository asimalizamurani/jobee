using jobee.Models;
using Microsoft.EntityFrameworkCore;

namespace jobee.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Interview> Interviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vacancy-Interview relationship
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Vacancy)
                .WithMany() // No cyclic reference to prevent serialization issues
                .HasForeignKey(i => i.VacancyId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a vacancy deletes its interviews

            // Applicant-Interview relationship
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Applicant)
                .WithMany()
                .HasForeignKey(i => i.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent unintended deletions

            // User (Interviewer)-Interview relationship
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Interviewer)
                .WithMany()
                .HasForeignKey(i => i.InterviewerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
