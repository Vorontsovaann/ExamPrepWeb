using Microsoft.AspNetCore.Mvc;
using ExamPrepWeb.Services;
using ExamPrepWeb.Models;

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
    }
}