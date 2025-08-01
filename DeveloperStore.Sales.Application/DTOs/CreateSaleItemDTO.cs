using System.ComponentModel.DataAnnotations;

namespace DeveloperStore.Sales.Application.DTOs
{
    public class CreateSaleItemDTO
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, 20, ErrorMessage = "A quantidade deve estar entre 1 e 20 itens.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "O preço unitário é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser um valor positivo.")]
        public decimal UnitPrice { get; set; }
    }
}