using Microsoft.AspNetCore.Mvc;
using ExamPrepWeb.Services;
using System.Text.Json;

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
        public async Task<ActionResult<EnrollmentResponse>> Enroll([FromBody] JsonElement request)
        {
            try
            {
                // Логируем что пришло
                _logger.LogInformation("Получен запрос: {Request}", request.GetRawText());

                string fio = "";
                string tel = "";
                string email = "";
                int courseId = 0;

                // Пробуем получить данные разными способами
                if (request.TryGetProperty("fio", out var fioProp))
                    fio = fioProp.GetString() ?? "";
                
                if (request.TryGetProperty("tel", out var telProp))
                    tel = telProp.GetString() ?? "";
                
                if (request.TryGetProperty("eml", out var emlProp))
                    email = emlProp.GetString() ?? "";
                else if (request.TryGetProperty("email", out var emailProp))
                    email = emailProp.GetString() ?? "";
                
                if (request.TryGetProperty("courseId", out var idProp))
                    courseId = idProp.GetInt32();

                _logger.LogInformation("Распарсено: Fio={Fio}, Tel={Tel}, Email={Email}, CourseId={CourseId}", 
                    fio, tel, email, courseId);

                if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest(new EnrollmentResponse 
                    { 
                        Success = false, 
                        Message = $"Пустые поля: Fio='{fio}', Email='{email}'" 
                    });
                }

                var result = await _courseService.EnrollStudentAsync(fio, tel, email, courseId);

                return Ok(new EnrollmentResponse 
                { 
                    Success = result.Success, 
                    Message = result.Message 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при записи");
                return StatusCode(500, new EnrollmentResponse 
                { 
                    Success = false, 
                    Message = $"Ошибка сервера: {ex.Message}" 
                });
            }
        }

        public class EnrollmentResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = "";
        }
    }
}