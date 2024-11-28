
using CadastroVeiculos.Dominio.Enuns;

namespace CadastroVeiculos.Dominio.DTO;

public class AdministradorDto
{
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
    public Perfil Perfil { get; set; } = default!;
}
