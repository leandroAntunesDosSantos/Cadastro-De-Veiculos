using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CadastroVeiculos.Dominio.DTO;
using CadastroVeiculos.Dominio.Entidades;
using CadastroVeiculos.Dominio.Intefaces;
using CadastroVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;

namespace CadastroVeiculos.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        public readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto)
        {
            _contexto = contexto;
        }
       
        public Administrador? Login(LoginDTO loginDto)
        {
            var administrador = _contexto.Administradores.FirstOrDefault(x => x.Email == loginDto.Email && x.Senha == loginDto.Senha);
            if (administrador == null)
            {
                return null;
            }
            return administrador;
        }
    }
}