var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/", () =>
{
    return "Hello World!";
});


app.MapPost("/login", (CadastroVeiculos.Dominio.DTO.LoginDTO LoginDto) =>
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


