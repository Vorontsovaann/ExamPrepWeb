using FluentValidation;
using ExamPrepWeb.Models;  // ← ИСПОЛЬЗУЕМ Models!

namespace ExamPrepWeb.Validators
{
    /// <summary>
    /// Валидатор запроса на запись студента.
    /// </summary>
    public class EnrollmentRequestValidator : AbstractValidator<EnrollmentRequest>
    {
        public EnrollmentRequestValidator()
        {
            RuleFor(x => x.Fio)
                .NotEmpty().WithMessage("ФИО обязательно")
                .MinimumLength(2).WithMessage("ФИО от 2 символов")
                .MaximumLength(100).WithMessage("ФИО не более 100 символов");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email")
                .MaximumLength(150).WithMessage("Email не более 150 символов");

            RuleFor(x => x.Tel)
                .NotEmpty().WithMessage("Телефон обязателен");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Неверный ID курса");
        }
    }
}