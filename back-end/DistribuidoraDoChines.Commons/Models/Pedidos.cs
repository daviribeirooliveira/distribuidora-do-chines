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
    public class Pedidos
    {
        public Pedidos()
        {
            Detalhes = new HashSet<Detalhes>();
        }

        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdCliente { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdClienteEndereco { get; set; }

        [Required(ErrorMessage = "Forma de pagamento é obrigatória")]
        public uint IdTiposDePagamento { get; set; }

        public DateTime? Data { get; set; }

        [NotMapped]
        public string ValorEmReais { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public decimal Valor { get; set; }

        [NotMapped]
        public string ValorFreteEmReais { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public decimal ValorFrete { get; set; }

        [NotMapped]
        public string TrocoEmReais { get; set; }

        public decimal? Troco { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Clientes IdClienteNavigation { get; set; }
        public virtual Enderecos IdClienteEnderecoNavigation { get; set; }
        public virtual TiposDePagamento IdTiposDePagamentoNavigation { get; set; }
        public virtual ICollection<Detalhes> Detalhes { get; set; }

        public string GetStatusColor()
        {
            return Status switch
            {
                "Em Atendimento" => "#e0a800",
                "Finalizado" => "#28a745",
                _ => "#dc3545"
            };
        }
    }
}