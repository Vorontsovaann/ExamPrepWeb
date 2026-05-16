using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamPrepWeb.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        [Column(TypeName = "datetime")]
        [Display(Name = "Дата записи")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}