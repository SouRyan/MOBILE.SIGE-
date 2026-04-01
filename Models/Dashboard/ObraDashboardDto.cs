using MOBILE.SIGE.Models.Enums;

namespace MOBILE.SIGE.Models.Dashboard;

public class ObraDashboardDto
{
    public int IdObra { get; set; }
    public string Nome { get; set; } = string.Empty;
    public StatusObra StatusObra { get; set; }
    public float PercentualMedicao { get; set; }
    public float PercentualProducao { get; set; }
}
