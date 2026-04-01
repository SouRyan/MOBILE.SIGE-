using System.ComponentModel.DataAnnotations;

namespace MOBILE.SIGE.Models.TipoUsuario
{
    public class TipoUsuarioCreateDto
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100)]
        public string NomeTipoUsuario { get; set; } = string.Empty;
    }
}
