using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroVeiculos.Dominio.DTO;
using CadastroVeiculos.Dominio.Entidades;

namespace CadastroVeiculos.Dominio.Intefaces
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


