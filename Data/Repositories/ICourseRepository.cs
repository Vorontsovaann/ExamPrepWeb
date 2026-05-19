using ExamPrepWeb.Models;

namespace ExamPrepWeb.Data.Repositories
{
    public interface ICourseRepository
    {
        // Курсы
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        
        // Студенты (НОВЫЕ МЕТОДЫ)
        Task<Student?> GetStudentByEmailAsync(string email);
        Task AddStudentAsync(Student student);
        
        // Записи
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task SaveChangesAsync();
    }
}