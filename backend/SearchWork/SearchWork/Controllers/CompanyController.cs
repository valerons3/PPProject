using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Models.DTO;
using SearchWork.Services;
using System.Drawing.Text;
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

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDTO model)
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
                return Unauthorized("Доступ запрещен. Требуется роль 'Employer'");
            }

            if (!int.TryParse(jwtToken.Payload["nameid"]?.ToString(), out int userId))
            {
                return Unauthorized("Ошибка получения идентификатора пользователя.");
            }

            var result = await companyService.CreateCompany(userId, model);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);

        }
    }
}
