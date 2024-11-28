using CadastroVeiculos.Dominio.Enuns;

namespace CadastroVeiculos.Dominio.ModelViews;

public record AdministradorModelView
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Perfil { get; set; }
}
