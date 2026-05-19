using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Models
{
    /// <summary>Связь студента с курсом (запись на курс).</summary>
    [Index(nameof(StudentId), nameof(CourseId), IsUnique = true, Name = "IX_UniqueEnrollment")]
    public class Enrollment
    {
        /// <summary>Уникальный идентификатор записи.</summary>
        [Key]
        public int EnrollmentId { get; set; }

        /// <summary>Идентификатор студента.</summary>
        [Required(ErrorMessage = "Идентификатор студента обязателен")]
        [Display(Name = "Студент")]
        public int StudentId { get; set; }

        /// <summary>Идентификатор курса.</summary>
        [Required(ErrorMessage = "Идентификатор курса обязателен")]
        [Display(Name = "Курс")]
        public int CourseId { get; set; }

        /// <summary>Дата и время записи (UTC).</summary>
        [Column(TypeName = "datetime2")]
        [Display(Name = "Дата записи")]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        /// <summary>Навигационное свойство: студент.</summary>
        [JsonIgnore]
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        /// <summary>Навигационное свойство: курс.</summary>
        [JsonIgnore]
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}