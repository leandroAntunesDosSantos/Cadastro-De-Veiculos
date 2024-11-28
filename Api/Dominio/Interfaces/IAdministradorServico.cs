using CadastroVeiculos.Dominio.DTO;
using CadastroVeiculos.Dominio.Entidades;

namespace CadastroVeiculos.Dominio.Interfaces
{

    public interface IAdministradorServico
    {
        Administrador? Login(LoginDto loginDto);

        void Incluir(Administrador administrador);

        void Alterar(Administrador administrador);

        void Excluir(int id);

        List<Administrador> Listar(int? pagina, int? tamanhoPagina);


    }
}


