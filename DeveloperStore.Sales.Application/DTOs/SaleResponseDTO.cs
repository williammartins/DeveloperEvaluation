namespace DeveloperStore.Sales.Application.DTOs
{
    public class SaleResponseDTO
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItemResponseDTO> Items { get; set; }
    }
}
