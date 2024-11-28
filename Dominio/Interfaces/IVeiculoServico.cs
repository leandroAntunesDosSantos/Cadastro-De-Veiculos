using CadastroVeiculos.Dominio.Entidades;

namespace CadastroVeiculos.Dominio.Interfaces
{

    public interface IVeiculoServico
    {
       List<Veiculo> ListarVeiculos(int pagina, string? nome=null, string? marca=null);
       Veiculo? BuscaPorId(int id);
       void Incluir(Veiculo veiculo);
       void Atualizar(Veiculo veiculo);
       void Apagar(Veiculo veiculo);
    }
}


