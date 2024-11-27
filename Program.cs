using CadastroVeiculos.Infraestrutura.DB;
using CadastroVeiculos.Dominio.DTO;
using Microsoft.EntityFrameworkCore;
using CadastroVeiculos.Dominio.Servicos;
using CadastroVeiculos.Dominio.Intefaces;
using Microsoft.AspNetCore.Mvc;
using CadastroVeiculos.Dominio.ModelViews;
using CadastroVeiculos.Dominio.Entidades;

#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AdministradorServico, AdministradorServico>();
builder.Services.AddScoped<VeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administradores
app.MapPost("/administradores/login",  ([FromBody] LoginDTO loginDto, [FromServices] AdministradorServico administradorServico) =>
{
    if(administradorServico.Login(loginDto) != null)
    {
        return Results.Ok("Login efetuado com sucesso");
    }
    return Results.NotFound("Usuário não encontrado");
});
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculoDto veiculoDto, [FromServices] VeiculoServico veiculoServico) =>
{
    var veiculo = new Veiculo(veiculoDto.Nome, veiculoDto.Marca, veiculoDto.Ano);
    veiculoServico.Incluir(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
    
});

app.MapGet("/veiculos", ([FromQuery] int pagina, [FromQuery] string? nome, [FromQuery] string? marca, [FromServices] VeiculoServico veiculoServico) =>
{
    return Results.Ok(veiculoServico.ListarVeiculos(pagina, nome, marca));
});
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion

