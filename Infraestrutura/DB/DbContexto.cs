using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroVeiculos.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CadastroVeiculos.Infraestrutura.DB
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration _configuracaoAppSttings;
        public DbContexto(IConfiguration configuracaoAppSttings)
        {
            _configuracaoAppSttings = configuracaoAppSttings;
        }
       
        public DbSet<Administrador> Administradores { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSttings.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
                }
            }
        }
    }
}