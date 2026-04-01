using System.ComponentModel.DataAnnotations;

namespace MOBILE.SIGE.Models.Usuario
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}
