
using System.ComponentModel.DataAnnotations;

namespace MOBILE.SIGE.Models.ProducaoFamilia;

public class ProducaoFamiliaFinalizarDto
{
    [StringLength(500)]
    public string? Observacoes { get; set; }
}
