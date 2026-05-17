using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Models;

namespace ExamPrepWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Каскадное удаление: Если удалили Студента -> удаляем его записи
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ограничительное удаление: Нельзя удалить Курс, если есть студенты
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Динамические даты: курсы всегда начинаются в будущем
            var baseDate = DateTime.Now.AddMonths(3);

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = 1,
                    Title = "Математика ЕГЭ",
                    Subject = "Математика",
                    Price = 5000m, // <-- decimal literal (буква m!)
                    StartDate = baseDate,
                    TeacherName = "Иванов И.И.",
                    Description = "Профильный уровень. Подготовка к ЕГЭ по математике."
                },
                new Course
                {
                    CourseId = 2,
                    Title = "Английский язык ЕГЭ",
                    Subject = "Английский",
                    Price = 7500m, // <-- decimal literal
                    StartDate = baseDate.AddDays(14),
                    TeacherName = "Петрова А.С.",
                    Description = "Интенсивный курс английского языка для сдачи ЕГЭ."
                },
                new Course
                {
                    CourseId = 3,
                    Title = "Информатика ЕГЭ",
                    Subject = "Информатика",
                    Price = 6000m, // <-- decimal literal
                    StartDate = baseDate.AddMonths(1),
                    TeacherName = "Сидоров В.К.",
                    Description = "Программирование на Python и C++. Решение задач части 2."
                }
            );
        }
    }
}