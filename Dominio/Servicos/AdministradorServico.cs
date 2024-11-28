using CadastroVeiculos.Dominio.DTO;
using CadastroVeiculos.Dominio.Entidades;
using CadastroVeiculos.Dominio.Interfaces;
using CadastroVeiculos.Infraestrutura.DB;

namespace CadastroVeiculos.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public Administrador? Login(LoginDto loginDto)
        {
            var administrador = _contexto.Administradores.FirstOrDefault(x => x.Email == loginDto.Email && x.Senha == loginDto.Senha);
            if (administrador == null)
            {
                return null;
            }
            return administrador;
        }

        public void Incluir(Administrador administrador)
        {
            _contexto.Administradores.Add(administrador);
            _contexto.SaveChanges();
        }

        public void Alterar(Administrador administrador)
        {
            var administradorBanco = _contexto.Administradores.FirstOrDefault(x => x.Id == administrador.Id);
            if (administradorBanco != null)
            {
                administradorBanco.Email = administrador.Email;
                administradorBanco.Senha = administrador.Senha;
                administradorBanco.Perfil = administrador.Perfil;
                _contexto.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            var administrador = _contexto.Administradores.FirstOrDefault(x => x.Id == id);
            if (administrador != null)
            {
                _contexto.Administradores.Remove(administrador);
                _contexto.SaveChanges();
            }
        }

        public List<Administrador> Listar(int? pagina, int? tamanhoPagina)
        {
            if (pagina == null || tamanhoPagina == null)
            {
                return _contexto.Administradores.ToList();
            }
            return _contexto.Administradores.Skip((pagina.Value - 1) * tamanhoPagina.Value).Take(tamanhoPagina.Value).ToList();
        }

        public Administrador? BuscarPorId(int id)
        {
            return _contexto.Administradores.FirstOrDefault(x => x.Id == id);
        }

        public Administrador? BuscarPorEmail(string email)
        {
            return _contexto.Administradores.FirstOrDefault(x => x.Email == email);
        }

    }
}
