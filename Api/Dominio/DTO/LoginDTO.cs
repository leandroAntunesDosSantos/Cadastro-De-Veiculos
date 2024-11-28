namespace CadastroVeiculos.Dominio.DTO
{
    public class LoginDto
    {
        public string Email { get; set; } = default!;  // O sinal de exclamação é para indicar que o valor não pode ser nulo
        public string Senha { get; set; } = default!;
    }
}
