using ExamPrepWeb.Data;
using ExamPrepWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<CourseService> _logger;

        public CourseService(AppDbContext context, ILogger<CourseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Course>> GetAvailableCoursesAsync()
        {
            _logger.LogInformation("Получение списка курсов");
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourseDetailsAsync(int courseId)
        {
            _logger.LogInformation("Получение деталей курса {CourseId}", courseId);
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task<EnrollmentResult> EnrollStudentAsync(string fullName, string phone, string email, int courseId)
        {
            _logger.LogInformation("Попытка записи: {FullName}, {Email}, курс {CourseId}",
                fullName, email, courseId);

            // 1. Валидация входных данных
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Пустые поля: FullName={FullName}, Email={Email}", fullName, email);
                return new EnrollmentResult { Success = false, Message = "Заполните ФИО и Email" };
            }

            // Парсим ФИО
            var names = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = names.FirstOrDefault() ?? string.Empty;
            var lastName = names.Length > 1 ? names[1] : string.Empty;

            if (string.IsNullOrEmpty(lastName))
            {
                _logger.LogWarning("Некорректное ФИО: {FullName}", fullName);
                return new EnrollmentResult { Success = false, Message = "Укажите Фамилию и Имя полностью" };
            }

            // Нормализуем email
            string normalizedEmail = email.ToLower().Trim();

            try
            {
                // 2. Поиск или создание студента
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Email == normalizedEmail);

                if (student == null)
                {
                    _logger.LogInformation("Создание нового студента: {Email}", normalizedEmail);
                    student = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = normalizedEmail,
                        DateOfBirth = new DateTime(2000, 1, 1)
                    };
                    _context.Students.Add(student);
                    
                    // Сохраняем студента СНАЧАЛА чтобы получить StudentId
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Студент сохранен с ID: {StudentId}", student.StudentId);
                }
                else
                {
                    _logger.LogInformation("Найден существующий студент: {Email}, ID: {StudentId}",
                        normalizedEmail, student.StudentId);
                }

                // 3. Проверка дубликата записи
                var alreadyEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == student.StudentId && e.CourseId == courseId);

                if (alreadyEnrolled)
                {
                    _logger.LogWarning("Студент {Email} уже записан на курс {CourseId}",
                        normalizedEmail, courseId);
                    return new EnrollmentResult { Success = false, Message = "Вы уже записаны на этот курс" };
                }

                // 4. Проверка что курс существует
                var courseExists = await _context.Courses.AnyAsync(c => c.CourseId == courseId);
                if (!courseExists)
                {
                    _logger.LogError("Курс с ID {CourseId} не найден", courseId);
                    return new EnrollmentResult { Success = false, Message = "Курс не найден" };
                }

                // 5. Создание записи
                var enrollment = new Enrollment
                {
                    StudentId = student.StudentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow
                };

                _context.Enrollments.Add(enrollment);

                // 6. Сохраняем запись
                await _context.SaveChangesAsync();
                _logger.LogInformation("Запись создана: EnrollmentId={EnrollmentId}", enrollment.EnrollmentId);

                _logger.LogInformation("Студент {Email} успешно записан на курс {CourseId}",
                    normalizedEmail, courseId);

                return new EnrollmentResult
                {
                    Success = true,
                    Message = $"Спасибо, {firstName}! Ваша заявка на курс принята."
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка БД при записи студента {Email}", normalizedEmail);
                var innerError = ex.InnerException?.Message ?? ex.Message;
                return new EnrollmentResult { Success = false, Message = $"Ошибка БД: {innerError}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неожиданная ошибка при записи студента {Email}", normalizedEmail);
                return new EnrollmentResult { Success = false, Message = $"Ошибка: {ex.Message}" };
            }
        }
    }
}