using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace SearchWork.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser userService;
        public UsersController(IUser userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        /// <returns>Информация о пользователе</returns>
        /// <response code="401">Не верный токен/не удалось получить id пользователя из токена</response>
        /// <response code="400">Пользователь не существует</response>
        /// <response code="200">Информация о пользователе</response>
        [HttpGet]
        public async Task<IActionResult> GetInfoUserById()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Неверный токен" });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Payload["nameid"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Не удалось получить ID пользователя" });
            }

            var result = await userService.GetUserInfoByIdAsync(int.Parse(userId));
            if (!result.Success)
            {
                return BadRequest(new { message = "Не удалось найти пользователя" });
            }
            return Ok(result.user);
        }
    }
}
