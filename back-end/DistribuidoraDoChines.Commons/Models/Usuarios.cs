using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable ClassNeverInstantiated.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Usuarios
    {
        public Usuarios(uint id, string usuario, string senha, DateTime? createdAt, DateTime? updatedAt)
        {
            Id = id;
            Usuario = usuario;
            Senha = senha;
            UpdatedAt = updatedAt;
            CreatedAt = createdAt;
        }

        public uint Id { get; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Text, ErrorMessage = "{0} deve ser um campo de texto válido")]
        [StringLength(255, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 4)]
        public string Usuario { get; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
        [StringLength(20, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 8)]
        public string Senha { get; }

        public DateTime? CreatedAt { get; }
        public DateTime? UpdatedAt { get; }
    }
}