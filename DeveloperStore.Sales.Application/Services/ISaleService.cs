using DeveloperStore.Sales.Application.DTOs;

namespace DeveloperStore.Sales.Application.Services
{
    public interface ISaleService
    {
        Task<SaleResponseDTO> CreateSale(CreateSaleDTO createSaleDto);
        Task<SaleResponseDTO?> GetSaleById(Guid id);
        Task<IEnumerable<SaleResponseDTO>> GetAllSales();
        Task<SaleResponseDTO> UpdateSale(Guid id, CreateSaleDTO updateSaleDto);
        Task CancelSale(Guid id);
        Task CancelSaleItem(Guid saleId, Guid itemId);
    }
}