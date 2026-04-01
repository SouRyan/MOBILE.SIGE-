using System.ComponentModel.DataAnnotations;

namespace MOBILE.SIGE.Models.Medicao;

public class MedicaoIniciarDto
{
    [Required]
    public int IdResponsavel { get; set; }

    public DateTime? DataEstimadaConclusao { get; set; }

    [StringLength(200)]
    public string? Descricao { get; set; }
}
