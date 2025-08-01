namespace DeveloperStore.Sales.Application.DTOs
{
    public class SaleItemResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}
