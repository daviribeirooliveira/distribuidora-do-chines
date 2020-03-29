using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class TiposDePagamento
    {
        public TiposDePagamento()
        {
            Pedidos = new HashSet<Pedidos>();
        }

        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        public string Descricao { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Pedidos> Pedidos { get; set; }
    }
}