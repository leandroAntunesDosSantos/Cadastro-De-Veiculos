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
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/administradores/login",  ([FromBody] LoginDTO loginDto, [FromServices] AdministradorServico administradorServico) =>
{
    if(administradorServico.Login(loginDto) != null)
    {
        return Results.Ok("Login efetuado com sucesso");
    }
    return Results.NotFound("Usuário não encontrado");
}).WithTags("Administradores");
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculoDto veiculoDto, [FromServices] VeiculoServico veiculoServico) =>
{
    var Mensagem = new ErroValidacao();
    if(veiculoDto.Nome == null || veiculoDto.Marca == null || veiculoDto.Ano == 0)
    {
        Mensagem.Mensagens = "Preencha todos os campos";
        return Results.BadRequest(Mensagem);
    }
    var veiculo = new Veiculo(veiculoDto.Nome, veiculoDto.Marca, veiculoDto.Ano);
    veiculoServico.Incluir(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
    
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int pagina, [FromQuery] string? nome, [FromQuery] string? marca, [FromServices] VeiculoServico veiculoServico) =>
{
    return Results.Ok(veiculoServico.ListarVeiculos(pagina, nome, marca));
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, [FromServices] VeiculoServico veiculoServico) =>
{
    var Mensagem = new ErroValidacao();
    if(id == 0)
    {
        Mensagem.Mensagens = "Informe um id";
        return Results.BadRequest(Mensagem);
    }
    if(veiculoServico.BuscaPorId(id) == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(veiculoServico.BuscaPorId(id));
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDto veiculoDto, [FromServices] VeiculoServico veiculoServico) =>
{
    var Mensagem = new ErroValidacao();
    if(veiculoDto.Nome == null || veiculoDto.Marca == null || veiculoDto.Ano == 0)
    {
        Mensagem.Mensagens = "Preencha todos os campos";
        return Results.BadRequest(Mensagem);
    }
    var veiculo = veiculoServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();
    veiculo.Nome = veiculoDto.Nome;
    veiculo.Marca = veiculoDto.Marca;
    veiculo.Ano = veiculoDto.Ano;
    veiculoServico.Atualizar(veiculo);
    return Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, [FromServices] VeiculoServico veiculoServico) =>
{
    var Mensagem = new ErroValidacao();
    if(id == 0)
    {
        Mensagem.Mensagens = "Informe um id";
        return Results.BadRequest(Mensagem);
    }
    var veiculo = veiculoServico.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();
    veiculoServico.Apagar(veiculo);
    return Results.NoContent();
}).WithTags("Veiculos");
#endregion


#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion

