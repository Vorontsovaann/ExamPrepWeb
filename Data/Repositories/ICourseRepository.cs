using ExamPrepWeb.Models;

namespace ExamPrepWeb.Data.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task SaveChangesAsync();
    }
}