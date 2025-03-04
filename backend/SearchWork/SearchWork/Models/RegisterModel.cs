using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    
    public class RegisterModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Имя должно содержать минимум 3 символа.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string UserType { get; set; } = string.Empty; // "seeker" или "employer"
    }

}
