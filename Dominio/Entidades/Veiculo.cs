using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroVeiculos.Dominio.Entidades
{
    public class Veiculo
    {
        public Veiculo(string nome, string marca, int ano)
        {
            Nome = nome;
            Marca = marca;
            Ano = ano;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string Marca { get; set; } = default!;
        
        [Required]
        public int Ano { get; set; } = default!;


    }
}