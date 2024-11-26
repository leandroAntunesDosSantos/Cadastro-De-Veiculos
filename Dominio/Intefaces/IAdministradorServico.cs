using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroVeiculos.Dominio.DTO;
using CadastroVeiculos.Dominio.Entidades;

namespace CadastroVeiculos.Dominio.Intefaces
{

    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTO loginDto);
    }
}


