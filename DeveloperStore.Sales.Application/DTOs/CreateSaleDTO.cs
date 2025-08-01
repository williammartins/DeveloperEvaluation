using System.ComponentModel.DataAnnotations;

namespace DeveloperStore.Sales.Application.DTOs
{
    public class CreateSaleDTO
    {
        [Required(ErrorMessage = "O número da venda é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O número da venda deve ser um valor positivo.")]
        public int SaleNumber { get; set; }

        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "O ID da filial é obrigatório.")]
        public Guid BranchId { get; set; }

        [Required(ErrorMessage = "Os itens da venda são obrigatórios.")]
        [MinLength(1, ErrorMessage = "A venda deve conter pelo menos um item.")]
        public List<CreateSaleItemDTO> Items { get; set; }
    }
}