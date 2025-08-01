using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Domain;

namespace DeveloperStore.Sales.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SaleResponseDTO> CreateSale(CreateSaleDTO createSaleDto)
        {
            var existingSale = (await _saleRepository.GetAll()).FirstOrDefault(s => s.SaleNumber == createSaleDto.SaleNumber);
            if (existingSale != null)
            {
                throw new InvalidOperationException($"Não é possível cadastrar uma venda com o número '{createSaleDto.SaleNumber}', pois já existe uma venda com este número.");
            }

            var sale = new Sale(createSaleDto.SaleNumber, createSaleDto.CustomerId, createSaleDto.BranchId);

            foreach (var itemDto in createSaleDto.Items)
            {
                var quantity = new Quantity(itemDto.Quantity);
                var unitPrice = new Money(itemDto.UnitPrice);

                var saleItem = new SaleItem(itemDto.ProductId, quantity, unitPrice);
                sale.AddItem(saleItem);
            }

            await _saleRepository.Add(sale);
            await _saleRepository.UnitOfWorkCommit();

            return MapToSaleResponseDTO(sale);
        }

        public async Task<SaleResponseDTO?> GetSaleById(Guid id)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null) return null; 

            return MapToSaleResponseDTO(sale);
        }

        public async Task<IEnumerable<SaleResponseDTO>> GetAllSales()
        {
            var sales = await _saleRepository.GetAll();
            return sales.Select(MapToSaleResponseDTO).ToList();
        }

        public async Task<SaleResponseDTO> UpdateSale(Guid id, CreateSaleDTO updateSaleDto)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
            {
                throw new ArgumentException($"Sale with ID {id} not found.");
            }

            if (sale.IsCancelled)
            {
                throw new InvalidOperationException("Cannot update a cancelled sale.");
            }

            var itemsToRemove = sale.Items.Where(item => !updateSaleDto.Items.Any(dto => dto.ProductId == item.ProductId)).ToList();
            foreach (var item in itemsToRemove)
            {
                sale.RemoveItem(item.ProductId);
            }

            foreach (var itemDto in updateSaleDto.Items)
            {
                var existingItem = sale.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
                if (existingItem != null)
                {
                    sale.UpdateItem(itemDto.ProductId, new Quantity(itemDto.Quantity));
                }
                else
                {
                    var newItem = new SaleItem(itemDto.ProductId, new Quantity(itemDto.Quantity), new Money(itemDto.UnitPrice));
                    sale.AddItem(newItem);
                }
            }

            _saleRepository.Update(sale);
            await _saleRepository.UnitOfWorkCommit();

            return MapToSaleResponseDTO(sale);
        }

        public async Task CancelSale(Guid id)
        {
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
            {
                throw new ArgumentException($"Sale with ID {id} not found.");
            }
            sale.Cancel();
            _saleRepository.Update(sale);
            await _saleRepository.UnitOfWorkCommit();
        }

        public async Task CancelSaleItem(Guid saleId, Guid itemId)
        {
            var sale = await _saleRepository.GetById(saleId);
            if (sale == null)
            {
                throw new ArgumentException($"Sale with ID {saleId} not found.");
            }
            if (sale.IsCancelled)
            {
                throw new InvalidOperationException("Cannot cancel an item from a cancelled sale.");
            }

            sale.CancelItem(itemId);

            _saleRepository.Update(sale);
            await _saleRepository.UnitOfWorkCommit();
        }

        // Método auxiliar para mapear a entidade de domínio para o DTO de resposta
        private SaleResponseDTO MapToSaleResponseDTO(Sale sale)
        {
            return new SaleResponseDTO
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                BranchId = sale.BranchId,
                TotalAmount = sale.TotalAmount.Value,
                IsCancelled = sale.IsCancelled,
                Items = sale.Items.Select(item => new SaleItemResponseDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity.Value,
                    UnitPrice = item.UnitPrice.Value,
                    Discount = item.Discount.Value,
                    TotalItemAmount = item.TotalItemAmount.Value,
                    IsCancelled = item.IsCancelled
                }).ToList()
            };
        }
    }
}