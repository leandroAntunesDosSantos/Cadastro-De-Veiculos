using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroVeiculos.Dominio.ModelViews
{
    public struct Home
    {
        public string Doc { get =>"/swagger";}

        public string Mensagem { get => "Bem vindo ao sistema de cadastro de veículos"; }
    }
}