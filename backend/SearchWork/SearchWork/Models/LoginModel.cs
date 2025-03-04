using System.ComponentModel.DataAnnotations;

namespace SearchWork.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
