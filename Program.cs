using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CadastroVeiculos.Infraestrutura.DB;
using CadastroVeiculos.Dominio.DTO;
using Microsoft.EntityFrameworkCore;
using CadastroVeiculos.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;
using CadastroVeiculos.Dominio.ModelViews;
using CadastroVeiculos.Dominio.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

#region Builder

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt")["Key"];
if (string.IsNullOrEmpty(key))
{
    key = "12345";
}

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<AdministradorServico, AdministradorServico>();
builder.Services.AddScoped<VeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home").AllowAnonymous();
#endregion

#region Administradores

string GerarTokenJwt(Administrador administrador)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil)
    };
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}


app.MapPost("/administradores/login",  ([FromBody] LoginDto loginDto, [FromServices] AdministradorServico administradorServico) =>
{
    var adm = administradorServico.Login(loginDto);
    if(adm != null)
    {
        string token = GerarTokenJwt(administradorServico.Login(loginDto));
        return Results.Ok(new AdmLogado
        {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    }
    return Results.NotFound("Usuário não encontrado");
}).AllowAnonymous().WithTags("Administradores");


app.MapPost("/administradores", ([FromBody] AdministradorDto administradorDto, [FromServices] AdministradorServico administradorServico) =>
{
    var Mensagem = new ErroValidacao();
    var administradorModelView = new AdministradorModelView();
    if(administradorDto.Email == "" || administradorDto.Senha == "")
    {
        Mensagem.Mensagens = "Preencha todos os campos";
        return Results.BadRequest(Mensagem);
    }
    var administrador = new Administrador(administradorDto.Email, administradorDto.Senha, administradorDto.Perfil.ToString());
    administradorServico.Incluir(administrador);
    administradorModelView.Id = administrador.Id;
    administradorModelView.Email = administrador.Email;
    administradorModelView.Perfil = administrador.Perfil;
    return Results.Created($"/administradores/{administrador.Id}", administradorModelView);
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Administradores");


app.MapGet("/administradores", ([FromQuery] int pagina, [FromServices] AdministradorServico administradorServico) =>
{
    var administradoresModelView = new List<AdministradorModelView>();
    var administradores = administradorServico.Listar(pagina, 10);
    foreach (var administrador in administradores)
    {
        administradoresModelView.Add(new AdministradorModelView
        {
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });
    }
    return Results.Ok(administradoresModelView);
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Administradores");

app.MapDelete("/administradores/{id}", ([FromRoute] int id, [FromServices] AdministradorServico administradorServico) =>
{
    var Mensagem = new ErroValidacao();
    if(id == 0)
    {
        Mensagem.Mensagens = "Informe um id";
        return Results.BadRequest(Mensagem);
    }
    administradorServico.Excluir(id);
    return Results.NoContent();
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Administradores");


app.MapPut("/administradores/{id}", ([FromRoute] int id, [FromBody] AdministradorDto administradorDto, [FromServices] AdministradorServico administradorServico) =>
{
    var mensagem = new ErroValidacao();
    if(administradorDto.Email == null || administradorDto.Senha == null)
    {
        mensagem.Mensagens = "Preencha todos os campos";
        return Results.BadRequest(mensagem);
    }
    var administrador = administradorServico.BuscarPorId(id);
    if(administrador == null) return Results.NotFound();
    administrador.Email = administradorDto.Email;
    administrador.Senha = administradorDto.Senha;
    administrador.Perfil = administradorDto.Perfil.ToString();
    administradorServico.Alterar(administrador);
    return Results.Ok(administrador);

}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, [FromServices] AdministradorServico administradorServico) =>
{
    var administradorModelView = new AdministradorModelView();
    var administrador = administradorServico.BuscarPorId(id);
    if(administrador == null) return Results.NotFound();
    administradorModelView.Id = administrador.Id;
    administradorModelView.Email = administrador.Email;
    administradorModelView.Perfil = administrador.Perfil;
    return Results.Ok(administradorModelView);
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Administradores");
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

}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int pagina, [FromQuery] string? nome, [FromQuery] string? marca, [FromServices] VeiculoServico veiculoServico) =>
{
    return Results.Ok(veiculoServico.ListarVeiculos(pagina, nome, marca));
}).RequireAuthorization().WithTags("Veiculos");

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
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin, Editor, Visitante"}).WithTags("Veiculos");

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
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Veiculos");

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
}).RequireAuthorization(new AuthorizeAttribute{ Roles = "Admin"}).WithTags("Veiculos");
#endregion


#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion

