using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Enderecos
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdCliente { get; set; }

        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.PostalCode, ErrorMessage = "{0} deve ser um cep válido")]
        [StringLength(8, ErrorMessage = "{0} deve conter {1} números.", MinimumLength = 8)]
        public string Cep { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Nº é obrigatório")]
        public string Numero { get; set; }

        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Complemento { get; set; }

        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Referencia { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Clientes IdClienteNavigation { get; set; }
        public virtual ICollection<Pedidos> Pedidos { get; set; }
    }
}