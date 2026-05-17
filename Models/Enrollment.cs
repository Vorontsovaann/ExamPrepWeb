using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Models
{
    [Index(nameof(StudentId), nameof(CourseId), IsUnique = true, Name = "IX_UniqueEnrollment")]
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Column(TypeName = "datetime2")]
        [Display(Name = "Дата записи")]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [JsonIgnore]
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}