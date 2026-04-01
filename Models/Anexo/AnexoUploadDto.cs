using MOBILE.SIGE.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace MOBILE.SIGE.Models.Anexo;

public class AnexoUploadDto
{
    public IFormFile? Arquivo { get; set; }
    public TipoAnexo TipoAnexo { get; set; }
    public int? IdMedicao { get; set; }
    public int? IdProducaoFamilia { get; set; }
    public int IdUsuario { get; set; }
}
