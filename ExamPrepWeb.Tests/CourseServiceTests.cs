using ExamPrepWeb.Data;
using ExamPrepWeb.Models;
using ExamPrepWeb.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExamPrepWeb.Tests
{
    public class CourseServiceTests
    {
        private readonly AppDbContext _context;
        private readonly CourseService _service;
        private readonly Mock<ILogger<CourseService>> _loggerMock;

        public CourseServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _loggerMock = new Mock<ILogger<CourseService>>();
            _service = new CourseService(_context, _loggerMock.Object);
        }

        [Theory]
        [InlineData("", "test@mail.ru", 1)]
        [InlineData("Иванов", "", 1)]
        [InlineData(" ", "test@mail.ru", 1)]
        public async Task EnrollStudentAsync_InvalidInput_ShouldReturnError(string fullName, string email, int courseId)
        {
            var result = await _service.EnrollStudentAsync(fullName, "tel", email, courseId);
            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Заполните ФИО и Email");
        }

        [Fact]
        public async Task EnrollStudentAsync_SingleName_ShouldReturnError()
        {
            var result = await _service.EnrollStudentAsync("Иванов", "tel", "ivan@mail.ru", 1);
            
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Укажите Фамилию и Имя полностью");
        }

        [Fact]
        public async Task EnrollStudentAsync_ValidInput_ShouldReturnSuccess()
        {
            var course = new Course 
            { 
                CourseId = 1, 
                Title = "Тестовый курс", 
                Subject = "Математика", 
                Price = 1000m, 
                StartDate = DateTime.Now.AddDays(10), 
                TeacherName = "Тест" 
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var result = await _service.EnrollStudentAsync("Иванов Иван", "tel", "ivan@test.ru", 1);

            result.Success.Should().BeTrue();
            // Исправлено: проверяем что сообщение не пустое и содержит "Спасибо"
            result.Message.Should().NotBeNullOrWhiteSpace();
            result.Message.Should().Contain("Спасибо");
            result.Message.Should().Contain("Иванов");
            _context.Students.Should().HaveCount(1);
            _context.Enrollments.Should().HaveCount(1);
        }

        [Fact]
        public async Task EnrollStudentAsync_DuplicateEnrollment_ShouldReturnError()
        {
            var course = new Course 
            { 
                CourseId = 1, 
                Title = "Тестовый курс", 
                Subject = "Математика", 
                Price = 1000m, 
                StartDate = DateTime.Now.AddDays(10), 
                TeacherName = "Тест" 
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            await _service.EnrollStudentAsync("Иванов Иван", "tel", "ivan@test.ru", 1);

            var result = await _service.EnrollStudentAsync("Иванов Иван", "tel", "ivan@test.ru", 1);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Вы уже записаны на этот курс");
        }
    }
}