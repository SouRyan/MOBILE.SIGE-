namespace MOBILE.SIGE.Models.Enums;
public enum StatusObra
{
    Cadastrada = 1,
    Verificada = 2,
    EmMedicao = 3,
    EmProducao = 4,
    Concluida = 5
}

public enum StatusFamilia
{
    Pendente = 1,
    EmMedicao = 2,
    Medida = 3,
    EmProducao = 4,
    Produzida = 5
}

public enum StatusAtividade
{
    NaoIniciada = 1,
    EmAndamento = 2,
    Pausada = 3,
    Concluida = 4
}

public enum TipoCargo
{
    Gerente = 1,
    ResponsavelVerificacao = 2,
    ResponsavelMedicao = 3,
    ResponsavelProducao = 4
}

public enum TipoAnexo
{
    Medicao = 1,
    Producao = 2
}

public enum TipoNotificacao
{
    ObraVerificada = 1,
    FamiliaMedida = 2,
    FamiliaProduzida = 3,
    ObraConcluida = 4,
    ObraCriada = 5,
    FotoMedicaoEnviada = 6,
    FotoMedicaoAprovada = 7,
    ProducaoIniciada = 8,
    FamiliaParaMedir = 9
}

/// <summary>Valores alinhados à API (ex.: Medido = 2).</summary>
public enum StatusProducao
{
    Pendente = 0,
    ParaMedir = 1,
    Medido = 2,
    Concluido = 3,
    PendenteLegado = 4
}
