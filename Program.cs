using CadastroVeiculos.Infraestrutura.DB;
using CadastroVeiculos.Dominio.DTO;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

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


