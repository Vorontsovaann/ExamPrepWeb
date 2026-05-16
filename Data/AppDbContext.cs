using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Models;

namespace ExamPrepWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }
        
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course 
                { 
                    CourseId = 1, 
                    Title = "Математика ЕГЭ", 
                    Subject = "Математика", 
                    Price = 5000, 
                    StartDate = new DateTime(2026, 9, 1), 
                    TeacherName = "Иванов И.И.",
                    Description = "Подготовка к ЕГЭ по математике"
                },
                new Course 
                { 
                    CourseId = 2, 
                    Title = "Английский язык ЕГЭ", 
                    Subject = "Английский", 
                    Price = 7500, 
                    StartDate = new DateTime(2026, 9, 15), 
                    TeacherName = "Петрова А.С.",
                    Description = "Подготовка к ЕГЭ по английскому языку"
                },
                new Course 
                { 
                    CourseId = 3, 
                    Title = "Информатика ЕГЭ", 
                    Subject = "Информатика", 
                    Price = 6000, 
                    StartDate = new DateTime(2026, 10, 1), 
                    TeacherName = "Сидоров В.К.",
                    Description = "Подготовка к ЕГЭ по информатике"
                }
            );
        }
    }
}