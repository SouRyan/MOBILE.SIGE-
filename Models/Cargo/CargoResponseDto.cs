using MOBILE.SIGE.Models.Enums;

namespace MOBILE.SIGE.Models.Cargo;

public class CargoResponseDto
{
    public int IdCargo { get; set; }
    public TipoCargo TipoCargo { get; set; }
    public string DescricaoCargo { get; set; } = string.Empty;
}
