using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Models
{
    /// <summary>Модель учебного курса.</summary>
    [Index(nameof(Title), Name = "IX_Course_Title")]
    public class Course
    {
        /// <summary>Уникальный идентификатор курса.</summary>
        [Key]
        public int CourseId { get; set; }

        /// <summary>Название курса (от 3 до 100 символов).</summary>
        [Required(ErrorMessage = "Название курса обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название от 3 до 100 символов")]
        [Display(Name = "Название курса")]
        public string Title { get; set; } = string.Empty;

        /// <summary>Предмет курса.</summary>
        [Required(ErrorMessage = "Предмет обязателен")]
        [StringLength(50, ErrorMessage = "Предмет не более 50 символов")]
        [Display(Name = "Предмет")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>Стоимость курса в рублях.</summary>
        [Required(ErrorMessage = "Укажите стоимость")]
        [Range(100, 99999999.99, ErrorMessage = "Цена должна быть от 100 руб.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Цена (руб.)")]
        public decimal Price { get; set; }

        /// <summary>Дата начала курса.</summary>
        [Required(ErrorMessage = "Укажите дату начала")]
        [Range(typeof(DateTime), "1/1/2024", "1/1/2035", ErrorMessage = "Дата начала должна быть в периоде 2024-2035")]
        [Column(TypeName = "date")]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        /// <summary>ФИО преподавателя.</summary>
        [Required(ErrorMessage = "Укажите преподавателя")]
        [StringLength(100, ErrorMessage = "ФИО преподавателя не более 100 символов")]
        [Display(Name = "Преподаватель")]
        public string TeacherName { get; set; } = string.Empty;

        /// <summary>Описание курса.</summary>
        [StringLength(500, ErrorMessage = "Описание не более 500 символов")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        /// <summary>Записи студентов на курс.</summary>
        public ICollection<Enrollment>? Enrollments { get; set; }

        /// <summary>Отформатированная цена для отображения.</summary>
        [NotMapped]
        public string DisplayPrice => Price.ToString("N0", new CultureInfo("ru-RU")) + " ₽";
    }
}