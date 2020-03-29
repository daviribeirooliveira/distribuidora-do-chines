using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Clientes
    {
        public Clientes()
        {
            Enderecos = new HashSet<Enderecos>();
            Pedidos = new HashSet<Pedidos>();
            Telefones = new HashSet<Telefones>();
        }

        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [EmailAddress(ErrorMessage = "{0} deve ser um email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
        [StringLength(20, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 8)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "{0} é obrigatória")]
        public bool Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Enderecos> Enderecos { get; set; }
        public ICollection<Pedidos> Pedidos { get; set; }
        public ICollection<Telefones> Telefones { get; set; }
    }
}