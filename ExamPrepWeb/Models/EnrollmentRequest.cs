using System.ComponentModel.DataAnnotations;

namespace ExamPrepWeb.Models
{
    /// <summary>
    /// Запрос на запись студента на курс.
    /// </summary>
    public class EnrollmentRequest
    {
        /// <summary>ФИО студента.</summary>
        [Required]
        public string Fio { get; set; } = string.Empty;

        /// <summary>Телефон.</summary>
        [Required]
        public string Tel { get; set; } = string.Empty;

        /// <summary>Email.</summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>Идентификатор курса.</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }
    }

    /// <summary>
    /// Ответ на запрос записи.
    /// </summary>
    public class EnrollmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}