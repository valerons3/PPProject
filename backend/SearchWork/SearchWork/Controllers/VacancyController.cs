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

        [HttpGet("company")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCompanyVacancies([FromQuery] string companyName)
        {
            if (string.IsNullOrEmpty(companyName)) { return BadRequest("Нужно передать название компании"); }

            var companyVacancies = await vacancyService.GetAllVacanciesCompanyAsync(companyName);
            if (companyVacancies == null) { return NotFound(new { message = "Вакансии для указанной компании не найдены"}) ; }

            return Ok(companyVacancies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewVacancyAsync(VacancyDTO model)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Не верный токен");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var role = jwtToken.Payload["role"]?.ToString();
            if (role != "Employer")
            {
                return Unauthorized("Доступ запрещён. Требуется роль 'Employer'");
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            if (userIdClaim == null)
            {
                return Unauthorized("Не удалось получить ID пользователя из токена");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Некорректный формат ID пользователя в токене");
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
