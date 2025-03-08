using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Models.DTO;
using SearchWork.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SearchWork.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory categoryService;

        public CategoryController(ICategory category)
        {
            categoryService = category;
        }

        // 🔹 Подтверждение добавления категории (PUT)
        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmCategoryRequestAsync(CategoryDTO model)
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

            if (role == "Seeker" || role == "Employer")
            {
                return Forbid("Доступ запрещён. Требуется роль 'Admin'");
            }

            var result = await categoryService.ConfirmationAddingCategoryAsync(model);
            if (!result)
            {
                return BadRequest($"Категории: \"{model.CategoryName}\" не существует в запросах на добавление категории");
            }
            return Ok($"Категория \"{model.CategoryName}\" добавлена в список категорий");
        }

        // 🔹 Создание запроса на добавление категории (POST)
        [HttpPost("add-request")]
        public async Task<IActionResult> CreateCategoryRequestAsync(CategoryDTO model)
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

            if (role == "Seeker")
            {
                return Forbid("Доступ запрещён. Требуется роль 'Employer' или 'Admin'");
            }

            var result = await categoryService.AddCategoryRequestAsync(model);

            if (result.Item1 == false)
            {
                return BadRequest(result.Item2);
            }

            return Ok(result.Item2);
        }
    }
}
