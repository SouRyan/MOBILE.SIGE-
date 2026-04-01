using System.ComponentModel.DataAnnotations;
using MOBILE.SIGE.Models.Enums;

namespace MOBILE.SIGE.Models.Cargo;

public class CargoCreateDto
{
    public TipoCargo TipoCargo { get; set; }

    [Required]
    [StringLength(100)]
    public string DescricaoCargo { get; set; } = string.Empty;
}
