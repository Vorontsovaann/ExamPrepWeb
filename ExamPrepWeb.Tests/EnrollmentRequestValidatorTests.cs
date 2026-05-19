using ExamPrepWeb.Models;
using ExamPrepWeb.Validators;
using FluentAssertions;
using Xunit;

namespace ExamPrepWeb.Tests
{
    public class EnrollmentRequestValidatorTests
    {
        private readonly EnrollmentRequestValidator _validator = new();

        [Fact]
        public void Validate_EmptyFio_ShouldFail()
        {
            var request = new EnrollmentRequest 
            { 
                Fio = "", 
                Email = "test@mail.ru", 
                Tel = "+79991234567", 
                CourseId = 1 
            };
            
            var result = _validator.Validate(request);
            
            result.IsValid.Should().BeFalse();
            // Исправлено: проверяем что есть хотя бы одна ошибка для Fio
            result.Errors.Should().Contain(e => e.PropertyName == nameof(EnrollmentRequest.Fio));
        }

        [Fact]
        public void Validate_InvalidEmail_ShouldFail()
        {
            var request = new EnrollmentRequest 
            { 
                Fio = "Иванов И.И.", 
                Email = "invalid-email", 
                Tel = "+79991234567", 
                CourseId = 1 
            };
            
            var result = _validator.Validate(request);
            
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(EnrollmentRequest.Email));
        }

        [Fact]
        public void Validate_CourseIdLessThanOne_ShouldFail()
        {
            var request = new EnrollmentRequest 
            { 
                Fio = "Иванов И.И.", 
                Email = "test@mail.ru", 
                Tel = "+79991234567", 
                CourseId = 0 
            };
            
            var result = _validator.Validate(request);
            
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(EnrollmentRequest.CourseId));
        }

        [Fact]
        public void Validate_ValidRequest_ShouldPass()
        {
            var request = new EnrollmentRequest 
            { 
                Fio = "Иванов Иван Иванович", 
                Email = "ivanov@example.com", 
                Tel = "+7 (999) 123-45-67", 
                CourseId = 5 
            };
            
            var result = _validator.Validate(request);
            
            result.IsValid.Should().BeTrue();
        }
    }
}