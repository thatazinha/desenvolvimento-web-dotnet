using System.ComponentModel.DataAnnotations;

namespace DW01.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Range(0.01, 999999.99,
        ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

    }
}
