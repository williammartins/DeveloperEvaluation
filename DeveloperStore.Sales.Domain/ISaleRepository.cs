using DeveloperStore.Sales.Domain.Interfaces;

namespace DeveloperStore.Sales.Domain
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<Sale> GetById(Guid id);
        Task<IEnumerable<Sale>> GetAll();
        Task Add(Sale sale);
        void Update(Sale sale);
        void Remove(Sale sale); 
    }
}