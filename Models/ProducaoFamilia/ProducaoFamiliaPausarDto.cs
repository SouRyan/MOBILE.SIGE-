using System.ComponentModel.DataAnnotations;

namespace MOBILE.SIGE.Models.ProducaoFamilia;

public class ProducaoFamiliaPausarDto
{
    [StringLength(500)]
    public string? Observacoes { get; set; }
}
