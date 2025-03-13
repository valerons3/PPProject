using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchWork.Models.DTO;
using SearchWork.Services;
using SearchWork.Services.Interfaces;
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

        /// <summary>
        /// Список всех категорий
        /// </summary>
        /// <returns>Возвращает список всех категорий</returns>
        /// <response code="200">Список категорий, если нет то пустой список</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await categoryService.GetAllCategoryAsync();

            if (categories == null) { return Ok(new List<CategoryDTO>()); }
            return Ok(categories);
        }


        /// <summary>
        /// Список всех вакансий по категории
        /// </summary>
        /// <returns>Возвращает список всех вакансий по категории</returns>
        /// <response code="400">Название категории пустое</response>
        /// <response code="404">Вакансий по категории не найдено</response>
        /// <response code="200">Список вакансий по категории</response>
        [HttpGet("vacancies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllVacanciesByCategoryAsync([FromQuery] string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Название категории не может быть пустым.");
            }

            var vacancies = await categoryService.GetAllVacancyByCategoryAsync(categoryName);

            if (vacancies == null || !vacancies.Any())
            {
                return NotFound("Вакансии по указанной категории не найдены.");
            }

            return Ok(vacancies);
        }

        /// <summary>
        /// Подтверждение на добавление категории (только для Админов)
        /// </summary>
        /// <returns>Сообщение об успехе</returns>
        /// <response code="401">Не верный токен</response>
        /// <response code="403">Доступ запрещён</response>
        /// <response code="400">Категории не существует в запросах на добавление</response>
        /// <response code="200">Категория добавлена</response>
        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmCategoryRequestAsync(CategoryDTO model)
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

        /// <summary>
        /// Создание запроса на добавление категории
        /// </summary>
        /// <returns>Сообщение об успехе</returns>
        /// <response code="401">Не верный токен</response>
        /// <response code="403">Доступ запрещён</response>
        /// <response code="400">Запрос на добавление категории уже существуетт</response>
        /// <response code="200">Запрос на добавление категории создан</response>
        [HttpPost("add-request")]
        public async Task<IActionResult> CreateCategoryRequestAsync(CategoryDTO model)
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
