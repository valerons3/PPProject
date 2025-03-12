using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Models.DTO;
using SearchWork.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace SearchWork.Controllers
{
    [Route("api/company")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCompanyByUserIdAsync()
        {
            var userId = GetUserIdFromToken(out string role);

            if (userId == null)
            {
                return Unauthorized("Не верный токен или ошибка получения идентификатора пользователя.");
            }

            if (role != "Employer")
            {
                return Unauthorized("Доступ запрещен. Требуется роль 'Employer'");
            }

            var result = await companyService.FindCompanyByIdAsync(userId.Value);
            if (result == null)
            {
                return NotFound("Компания не найдена.");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDTO model)
        {
            var userId = GetUserIdFromToken(out string role);

            if (userId == null)
            {
                return Unauthorized(new { message = "Не верный токен или ошибка получения идентификатора пользователя." });
            }

            if (role != "Employer")
            {
                return Unauthorized(new { message = "Доступ запрещен. Требуется роль 'Employer'." });
            }

            var result = await companyService.CreateCompany(userId.Value, model);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        private int? GetUserIdFromToken(out string role)
        {
            role = null;

            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                Console.WriteLine("Токен не найден или некорректный.");
                return null;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            role = jwtToken.Payload["role"]?.ToString();
            return int.TryParse(jwtToken.Payload["nameid"]?.ToString(), out int userId) ? userId : (int?)null;
        }

    }
}
