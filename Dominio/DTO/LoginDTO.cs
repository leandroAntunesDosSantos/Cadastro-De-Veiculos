using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroVeiculos.Dominio.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; } = default!;  // O sinal de exclamação é para indicar que o valor não pode ser nulo
        public string Senha { get; set; } = default!;
    }
}
