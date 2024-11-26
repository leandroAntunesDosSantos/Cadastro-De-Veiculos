var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/", () =>
{
    return "Hello World!";
});


app.MapPost("/login", (LoginDto LoginDto) =>
{
   if(LoginDto.Email == "admin@teste.com" && LoginDto.Senha == "123456")
   {
         return Results.Ok("Login efetuado com sucesso!");
    }
    else
    {
         return Results.Unauthorized();
    }

});

app.Run();


public class LoginDto
{
    public string Email { get; set; } = default!;  // O sinal de exclamação é para indicar que o valor não pode ser nulo
    public string Senha { get; set; } = default!;
}
