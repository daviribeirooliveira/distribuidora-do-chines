using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DistribuidoraDoChines.Commons.Models
{
    public class Credentials
    {
        [Required(ErrorMessage = "{0} é obrigatório")]
        [EmailAddress(ErrorMessage = "{0} deve ser um email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
        [StringLength(255, ErrorMessage = "{0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 8)]
        public string Senha { get; set; }
    }
}