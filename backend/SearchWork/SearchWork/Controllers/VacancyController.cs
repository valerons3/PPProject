using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Models.DTO;
using SearchWork.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SearchWork.Controllers
{
    [Route("api/vacancy")]
    [Authorize]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancy vacancyService;
        public VacancyController(IVacancy vacancyService)
        {
            this.vacancyService = vacancyService;
        }

        /// <summary>
        /// Вакансии компании
        /// </summary>
        /// <returns>Список вакансий компании</returns>
        /// <response code="400">Название компании пустое</response>
        /// <response code="404">Вакансии компании не найдены</response>
        /// <response code="200">Список всех вакансий компании</response>
        [HttpGet("company")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCompanyVacancies([FromQuery] string companyName)
        {
            if (string.IsNullOrEmpty(companyName)) { return BadRequest("Нужно передать название компании"); }

            var companyVacancies = await vacancyService.GetAllVacanciesCompanyAsync(companyName);
            if (companyVacancies == null) { return NotFound(new { message = "Вакансии для указанной компании не найдены"}) ; }

            return Ok(companyVacancies);
        }

        /// <summary>
        /// Создание вакансии
        /// </summary>
        /// <returns>Список вакансий компании</returns>
        /// <response code="401">Запрещён доступ/не верный токен</response>
        /// <response code="400">У пользователя нет компании/не верно переданная категория</response>
        /// <response code="200">Сообщение об успешном создании вакансии</response>
        [HttpPost]
        public async Task<IActionResult> CreateNewVacancyAsync(VacancyDTO model)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Не верный токен" });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var role = jwtToken.Payload["role"]?.ToString();
            if (role != "Employer")
            {
                return Unauthorized(new { message = "Доступ запрещён. Требуется роль 'Employer'"});
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Не удалось получить ID пользователя из токена" });
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Некорректный формат ID пользователя в токене" });
            }

            var result = await vacancyService.CreateVacancyAsync(model, userId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
