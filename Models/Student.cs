using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Models
{
    /// <summary>
    /// Модель сущности "Студент". Представляет пользователя системы.
    /// </summary>
    [Index(nameof(Email), IsUnique = true, Name = "IX_UniqueEmail")]
    public class Student
    {
        /// <summary>Уникальный идентификатор студента.</summary>
        [Key]
        public int StudentId { get; set; }

        /// <summary>Имя студента (от 2 до 100 символов).</summary>
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя от 2 до 100 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Фамилия студента (от 2 до 100 символов).</summary>
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Фамилия от 2 до 100 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>Электронная почта (уникальная).</summary>
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        [StringLength(150, ErrorMessage = "Email не более 150 символов")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Дата рождения студента.</summary>
        [Required(ErrorMessage = "Укажите дату рождения")]
        [Range(typeof(DateTime), "1/1/1940", "1/1/2015", ErrorMessage = "Некорректная дата рождения")]
        [Column(TypeName = "date")]
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>Коллекция записей на курсы.</summary>
        public ICollection<Enrollment>? Enrollments { get; set; }

        /// <summary>Полное имя (не сохраняется в БД).</summary>
        [NotMapped]
        [Display(Name = "Полное имя")]
        public string FullName => $"{LastName} {FirstName}".Trim();
    }
}