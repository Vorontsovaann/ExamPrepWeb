using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Models
{
    [Index(nameof(Email), IsUnique = true, Name = "IX_UniqueEmail")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(100)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите дату рождения")]
        [Range(typeof(DateTime), "1/1/1940", "1/1/2015", ErrorMessage = "Некорректная дата рождения")]
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}