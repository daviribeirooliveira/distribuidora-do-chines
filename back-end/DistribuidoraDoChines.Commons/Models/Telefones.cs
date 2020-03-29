using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Telefones
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdCliente { get; set; }

        [Phone(ErrorMessage = "{0} deve ser um campo de {0} válido")]
        [StringLength(8, ErrorMessage = "{0} deve conter {1} números.", MinimumLength = 8)]
        public string Residencial { get; set; }

        [Phone(ErrorMessage = "{0} deve ser um campo de {0} válido")]
        [StringLength(9, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} números.", MinimumLength = 8)]
        public string Comercial { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [Phone(ErrorMessage = "{0} deve ser um campo de {0} válido")]
        [StringLength(9, ErrorMessage = "{0} deve conter {1} números.", MinimumLength = 9)]
        public string Celular { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Clientes IdClienteNavigation { get; set; }
    }
}