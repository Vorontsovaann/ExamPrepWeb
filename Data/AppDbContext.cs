using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Models;

namespace ExamPrepWeb.Data
{
    /// <summary>Контекст базы данных приложения.</summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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

            // ️ SeedData(modelBuilder) УДАЛЕН отсюда!
        }

        /// <summary>Заполняет БД начальными данными при первом запуске.</summary>
        public static void SeedData(AppDbContext context)
        {
            if (context.Courses.Any()) return; // Защита от дублирования

            var startDate = new DateTime(2026, 09, 01); // Фиксированная дата

            context.Courses.AddRange(
                new Course
                {
                    CourseId = 1,
                    Title = "Математика ЕГЭ",
                    Subject = "Математика",
                    Price = 5000m,
                    StartDate = startDate,
                    TeacherName = "Иванов И.И.",
                    Description = "Профильный уровень."
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Английский язык ЕГЭ",
                    Subject = "Английский",
                    Price = 7500m,
                    StartDate = startDate.AddDays(14),
                    TeacherName = "Петрова А.С.",
                    Description = "Интенсивный курс."
                },
                new Course
                {
                    CourseId = 3,
                    Title = "Информатика ЕГЭ",
                    Subject = "Информатика",
                    Price = 6000m,
                    StartDate = startDate.AddMonths(1),
                    TeacherName = "Сидоров В.К.",
                    Description = "Python & C++."
                }
            );

            context.SaveChanges();
        }
    }
}