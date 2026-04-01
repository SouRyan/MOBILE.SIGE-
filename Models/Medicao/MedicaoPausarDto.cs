using System.ComponentModel.DataAnnotations;

namespace MOBILE.SIGE.Models.Medicao;

public class MedicaoPausarDto
{
    [StringLength(500)]
    public string? Observacoes { get; set; }
}
