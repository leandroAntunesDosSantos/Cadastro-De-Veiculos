namespace CadastroVeiculos.Dominio.ModelViews;

public record AdmLogado
{
    public string Email { get; set; }
    public string Perfil { get; set; }
    public string Token { get; set; }
}

