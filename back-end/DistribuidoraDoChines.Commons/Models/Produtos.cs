using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Produtos
    {
        public Produtos()
        {
            Detalhes = new HashSet<Detalhes>();
        }

        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdCategoria { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [NotMapped]
        [DataType(DataType.Currency, ErrorMessage = "{0} deve ser um valor válido")]
        public string PrecoEmReais { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Currency, ErrorMessage = "{0} deve ser um valor válido")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public bool Unidade { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public bool Status { get; set; }

        public byte[] Imagem { get; set; }

        [NotMapped] public string ImagemBlob { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Categorias IdCategoriaNavigation { get; set; }
        public virtual ICollection<Detalhes> Detalhes { get; set; }
    }
}