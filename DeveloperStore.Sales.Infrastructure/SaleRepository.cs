using DeveloperStore.Sales.Domain;
using DeveloperStore.Sales.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesDbContext _context;

        public SaleRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> GetById(Guid id)
        {
            // Inclui os itens da venda ao carregar a venda
            return await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAll()
        {
            // Inclui os itens da venda ao carregar todas as vendas
            return await _context.Sales
                                 .Include(s => s.Items)
                                 .ToListAsync();
        }

        public async Task Add(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public void Update(Sale sale)
        {
            // O EF Core rastreia as entidades e suas mudanças automaticamente
            // Se a entidade já está sendo rastreada e foi modificada, um Update explícito não é sempre necessário.
            // No entanto, para garantir que as alterações sejam percebidas, especialmente em cenários desconectados ou com muitos Owned Types,
            // é bom marcar o estado.
            _context.Sales.Update(sale);
        }

        public void Remove(Sale sale)
        {
            _context.Sales.Remove(sale);
        }

        // Implementação do Unit of Work Commit
        public async Task<int> UnitOfWorkCommit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}