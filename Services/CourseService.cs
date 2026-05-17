using ExamPrepWeb.Data;
using ExamPrepWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetAvailableCoursesAsync();
        Task<Course?> GetCourseDetailsAsync(int courseId);
        Task<EnrollmentResult> EnrollStudentAsync(string fullName, string phone, string email, int courseId);
    }

    public class EnrollmentResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAvailableCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourseDetailsAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task<EnrollmentResult> EnrollStudentAsync(string fullName, string phone, string email, int courseId)
        {
            // 1. Валидация
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
                return new EnrollmentResult { Success = false, Message = "Заполните ФИО и Email" };

            // Парсим ФИО
            var names = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = names.FirstOrDefault() ?? "";
            var lastName = names.Length > 1 ? names[1] : "";

            if (string.IsNullOrEmpty(lastName))
                return new EnrollmentResult { Success = false, Message = "Укажите Фамилию и Имя полностью" };

            try
            {
                // Нормализуем email (нижний регистр) - ВАЖНО!
                var normalizedEmail = email.ToLower().Trim();

                // 2. Поиск студента - простое сравнение (SQLite понимает)
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Email == normalizedEmail);

                if (student == null)
                {
                    student = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = normalizedEmail,
                        DateOfBirth = new DateTime(2000, 1, 1) // Заглушка
                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                }

                // 3. Проверка дубликата записи
                var alreadyEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == student.StudentId && e.CourseId == courseId);

                if (alreadyEnrolled)
                    return new EnrollmentResult { Success = false, Message = "Вы уже записаны на этот курс" };

                // 4. Создание записи
                var enrollment = new Enrollment
                {
                    StudentId = student.StudentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return new EnrollmentResult { Success = true, Message = $"Спасибо, {firstName}! Заявка принята." };
            }
            catch (DbUpdateException)
            {
                return new EnrollmentResult { Success = false, Message = "Ошибка базы данных." };
            }
            catch (Exception ex)
            {
                return new EnrollmentResult { Success = false, Message = $"Ошибка: {ex.Message}" };
            }
        }
    }
}