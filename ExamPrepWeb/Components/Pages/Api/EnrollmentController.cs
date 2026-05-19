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
            if (!ModelState.IsValid)
                return BadRequest(new EnrollmentResponse { Success = false, Message = "Неверные данные" });

            try
            {
                var result = await _courseService.EnrollStudentAsync(request.Fio, request.Tel, request.Email, request.CourseId);
                return Ok(new EnrollmentResponse { Success = result.Success, Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка API записи");
                return StatusCode(500, new EnrollmentResponse { Success = false, Message = "Внутренняя ошибка сервера" });
            }
        }

        public class EnrollmentRequest
        {
            [Required] public string Fio { get; set; } = string.Empty;
            [Required] public string Tel { get; set; } = string.Empty;
            [Required][EmailAddress] public string Email { get; set; } = string.Empty;
            [Required][Range(1, int.MaxValue)] public int CourseId { get; set; }
        }

        public class EnrollmentResponse
        {
            public bool Success { get; set; }
            public string? Message { get; set; } = string.Empty;
        }
    }
}