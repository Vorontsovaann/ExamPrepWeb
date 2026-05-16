using ExamPrepWeb.Data;
using ExamPrepWeb.Data.Repositories;
using ExamPrepWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPrepWeb.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetAvailableCoursesAsync();
        Task<Course?> GetCourseDetailsAsync(int courseId);
        Task<bool> EnrollStudentAsync(string firstName, string lastName, string email, int courseId);
    }
    
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly AppDbContext _context;
        
        public CourseService(ICourseRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        public async Task<List<Course>> GetAvailableCoursesAsync()
        {
            return await _repository.GetAllCoursesAsync();
        }
        
        public async Task<Course?> GetCourseDetailsAsync(int courseId)
        {
            return await _repository.GetCourseByIdAsync(courseId);
        }
        
        public async Task<bool> EnrollStudentAsync(string firstName, string lastName, string email, int courseId)
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == email);
            
            Student student;
            
            if (existingStudent != null)
            {
                student = existingStudent;
            }
            else
            {
                student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    DateOfBirth = DateTime.Now.AddYears(-18)
                };
                
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }
            
            var isEnrolled = await _repository.IsStudentEnrolledAsync(student.StudentId, courseId);
            if (isEnrolled)
            {
                return false;
            }
            
            var enrollment = new Enrollment
            {
                StudentId = student.StudentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now
            };
            
            await _repository.AddEnrollmentAsync(enrollment);
            await _repository.SaveChangesAsync();
            
            return true;
        }
    }
}