using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Categorias
    {
        public Categorias()
        {
            Produtos = new HashSet<Produtos>();
        }

        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public bool Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Produtos> Produtos { get; set; }
    }
}