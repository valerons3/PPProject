using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Models.Entity;
using SearchWork.Models.DTO;
using SearchWork.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using SearchWork.Services.Interfaces;

namespace SearchWork.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth authService;

        public AuthController(IAuth authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <returns>JWT-токен</returns>
        /// <response code="200">Возвращает JWT-токен</response>
        /// <response code="400">Пользователь с такой почтой уже существует</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await authService.Register(model);
            if (result == "Пользователь с такой почтой уже существует")
                return BadRequest(new { message = result });

            return Ok(new { token = result });
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <returns>JWT-токен</returns>
        /// <response code="200">Возвращает JWT-токен</response>
        /// <response code="401">Сообщение о неверном пароле или емейл</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var token = await authService.Login(model.Email, model.Password);
            if (token == null)
                return Unauthorized(new { message = "Не верный емейл или пароль" });

            return Ok(new { token });
        }
    }
}
