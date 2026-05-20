using ExamPrepWeb.Data.Repositories;
using ExamPrepWeb.Models;
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
        private readonly ICourseRepository _repository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository repository, ILogger<CourseService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<Course>> GetAvailableCoursesAsync()
        {
            _logger.LogInformation("Получение списка курсов");
            return await _repository.GetAllCoursesAsync();
        }

        public async Task<Course?> GetCourseDetailsAsync(int courseId)
        {
            _logger.LogInformation("Получение деталей курса {CourseId}", courseId);
            return await _repository.GetCourseByIdAsync(courseId);
        }

        public async Task<EnrollmentResult> EnrollStudentAsync(string fullName, string phone, string email, int courseId)
        {
            _logger.LogInformation("Попытка записи: {FullName}, {Email}, курс {CourseId}", fullName, email, courseId);

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Пустые поля: FullName={FullName}, Email={Email}", fullName, email);
                return new EnrollmentResult { Success = false, Message = "Заполните ФИО и Email" };
            }

            var names = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = names.FirstOrDefault() ?? string.Empty;
            var lastName = names.Length > 1 ? names[1] : string.Empty;

            if (string.IsNullOrEmpty(lastName))
            {
                _logger.LogWarning("Некорректное ФИО: {FullName}", fullName);
                return new EnrollmentResult { Success = false, Message = "Укажите Фамилию и Имя полностью" };
            }

            string normalizedEmail = email.ToLower().Trim();

            try
            {
                var student = await _repository.GetStudentByEmailAsync(normalizedEmail);

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
                    await _repository.AddStudentAsync(student);
                    await _repository.SaveChangesAsync();
                    _logger.LogInformation("Студент сохранен с ID: {StudentId}", student.StudentId);
                }

                if (await _repository.IsStudentEnrolledAsync(student.StudentId, courseId))
                {
                    _logger.LogWarning("Студент {Email} уже записан на курс {CourseId}", normalizedEmail, courseId);
                    return new EnrollmentResult { Success = false, Message = "Вы уже записаны на этот курс" };
                }

                var course = await _repository.GetCourseByIdAsync(courseId);
                if (course == null)
                {
                    _logger.LogError("Курс с ID {CourseId} не найден", courseId);
                    return new EnrollmentResult { Success = false, Message = "Курс не найден" };
                }

                var enrollment = new Enrollment
                {
                    StudentId = student.StudentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow
                };

                await _repository.AddEnrollmentAsync(enrollment);
                await _repository.SaveChangesAsync();

                return new EnrollmentResult
                {
                    Success = true,
                    Message = $"Спасибо, {firstName}! Ваша заявка на курс принята."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при записи студента {Email}", normalizedEmail);
                return new EnrollmentResult { Success = false, Message = $"Ошибка: {ex.Message}" };
            }
        }
    }
}