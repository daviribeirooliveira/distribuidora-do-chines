using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistribuidoraDoChines.Commons.Models
{
    public class Detalhes
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdPedido { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public uint IdProduto { get; set; }

        [NotMapped]
        public string ValorEmReais { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Currency, ErrorMessage = "{0} deve ser um valor válido")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} deve ser um campo numérico válido")]
        public uint Quantidade { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Pedidos IdPedidoNavigation { get; set; }
        public Produtos IdProdutoNavigation { get; set; }
    }
}