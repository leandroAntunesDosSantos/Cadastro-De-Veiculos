using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CadastroVeiculos.Dominio.DTO;
using CadastroVeiculos.Dominio.Entidades;
using CadastroVeiculos.Dominio.Intefaces;
using CadastroVeiculos.Infraestrutura.DB;
using Microsoft.EntityFrameworkCore;

namespace CadastroVeiculos.Dominio.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        public readonly DbContexto _contexto;
        public VeiculoServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public void Apagar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public Veiculo BuscaPorId(int id)
        {
            var veiculo = _contexto.Veiculos.FirstOrDefault(x => x.Id == id);
            if (veiculo == null)
            {
                throw new Exception("Veículo não encontrado");
            }
            return veiculo;
        }

        public void Incluir(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }

        public List<Veiculo> ListarVeiculos(int pagina = 1, string? nome = null, string? marca = null)
        {
            var veiculos = _contexto.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                veiculos = veiculos.Where(x => x.Nome.Contains(nome));
            }
            if (!string.IsNullOrEmpty(marca))
            {
                veiculos = veiculos.Where(x => x.Marca.Contains(marca));
            }
            return veiculos.Skip((pagina - 1) * 10).Take(10).ToList();
        }

    }
}