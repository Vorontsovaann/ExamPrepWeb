using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ExamPrepWeb.Services;

namespace ExamPrepWeb.Components.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(ICourseService courseService, ILogger<EnrollmentController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentResponse>> Enroll([FromBody] EnrollmentRequest request)
        {
            try
            {
                _logger.LogInformation("Получен API запрос на запись: {@Request}", request);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Неверные данные запроса");
                    return BadRequest(new EnrollmentResponse 
                    { 
                        Success = false, 
                        Message = "Неверные данные" 
                    });
                }

                var result = await _courseService.EnrollStudentAsync(
                    request.Fio, 
                    request.Tel, 
                    request.Email, 
                    request.CourseId
                );

                _logger.LogInformation("Результат записи: Success={Success}, Message={Message}", 
                    result.Success, result.Message);

                return Ok(new EnrollmentResponse 
                { 
                    Success = result.Success, 
                    Message = result.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке запроса на запись");
                return StatusCode(500, new EnrollmentResponse 
                { 
                    Success = false, 
                    Message = "Внутренняя ошибка сервера" 
                });
            }
        }

        public class EnrollmentRequest
        {
            [Required]
            public string Fio { get; set; } = "";
            
            [Required]
            public string Tel { get; set; } = "";
            
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
            
            [Required]
            [Range(1, int.MaxValue)]
            public int CourseId { get; set; }
        }

        public class EnrollmentResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
        }
    }
}