using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamPrepWeb.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Column(TypeName = "date")]
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }
        
        public ICollection<Enrollment>? Enrollments { get; set; }
        
        [NotMapped]
        [Display(Name = "Полное имя")]
        public string FullName => $"{LastName} {FirstName}";
    }
}