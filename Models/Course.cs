using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        
        [Required(ErrorMessage = "Название курса обязательно")]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Название курса")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Предмет обязателен")]
        [StringLength(50)]
        [Display(Name = "Предмет")]
        public string Subject { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99)]
        [Display(Name = "Цена (руб.)")]
        public decimal Price { get; set; }
        
        [Column(TypeName = "date")]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        
        [StringLength(100)]
        [Display(Name = "Преподаватель")]
        public string TeacherName { get; set; } = string.Empty;
        
        [StringLength(500)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}