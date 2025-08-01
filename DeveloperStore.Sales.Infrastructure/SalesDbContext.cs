using DeveloperStore.Sales.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

        // DbSet para a entidade principal Sale (Venda)
        public DbSet<Sale> Sales { get; set; }
        // DbSet para SaleItem, embora geralmente acessados via Sale.Items, é bom ter para consultas diretas se necessário
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamento para ignorar propriedades não persistentes
            // ou garantir que as propriedades das entidades sejam mapeadas corretamente
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)"); // Define um tamanho padrão para strings
            }

            // Mapeamento para Value Objects (propriedades complexas)
            // O Owned Type para Money e Quantity faz com que eles sejam mapeados
            // como colunas na mesma tabela da entidade que os contém (Sale ou SaleItem).
            modelBuilder.Entity<Sale>()
                .OwnsOne(s => s.TotalAmount, ta =>
                {
                    ta.Property(p => p.Value).HasColumnName("TotalAmount").HasColumnType("decimal(18,2)");
                });

            modelBuilder.Entity<SaleItem>()
                .OwnsOne(si => si.Quantity, q =>
                {
                    q.Property(p => p.Value).HasColumnName("Quantity");
                });

            modelBuilder.Entity<SaleItem>()
                .OwnsOne(si => si.UnitPrice, up =>
                {
                    up.Property(p => p.Value).HasColumnName("UnitPrice").HasColumnType("decimal(18,2)");
                });

            modelBuilder.Entity<SaleItem>()
                .OwnsOne(si => si.Discount, d =>
                {
                    d.Property(p => p.Value).HasColumnName("Discount").HasColumnType("decimal(18,2)");
                });

            modelBuilder.Entity<SaleItem>()
                .OwnsOne(si => si.TotalItemAmount, tia =>
                {
                    tia.Property(p => p.Value).HasColumnName("TotalItemAmount").HasColumnType("decimal(18,2)");
                });

            // Configuração do relacionamento entre Sale e SaleItem
            // Sale tem muitos SaleItems
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Items)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade); // Se a Sale for deletada, seus itens também o serão

            // Garante que o SaleNumber seja único
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.SaleNumber)
                .IsUnique();

            // Mapeamento adicional para propriedades que são listas privadas para forçar EF a usar o getter público
            modelBuilder.Entity<Sale>()
                .Metadata.FindNavigation(nameof(Sale.Items))
                .SetPropertyAccessMode(PropertyAccessMode.Field); // Acessa o campo privado _items

            base.OnModelCreating(modelBuilder);
        }

        // Sobrescreve SaveChangesAsync para implementar o Unit of Work
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Poderíamos adicionar lógica para disparar eventos de domínio aqui,
            // mas para este teste, não é necessário.
            var success = await base.SaveChangesAsync(cancellationToken);
            return success;
        }
    }
}