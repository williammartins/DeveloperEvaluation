namespace DeveloperStore.Sales.Domain.Interfaces
{
    // Interface base para todos os repositórios
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<int> UnitOfWorkCommit(); 
    }
}