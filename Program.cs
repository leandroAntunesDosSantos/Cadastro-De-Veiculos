using CadastroVeiculos.Infraestrutura.DB;
using CadastroVeiculos.Dominio.DTO;
using Microsoft.EntityFrameworkCore;
using CadastroVeiculos.Dominio.Servicos;
using CadastroVeiculos.Dominio.Intefaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AdministradorServico, AdministradorServico>();

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



app.MapPost("/login",  ([FromBody] LoginDTO loginDto, [FromServices] AdministradorServico administradorServico) =>
{
    if(administradorServico.Login(loginDto) != null)
    {
        return Results.Ok("Login efetuado com sucesso");
    }
    return Results.NotFound("Usuário não encontrado");
});



app.Run();


