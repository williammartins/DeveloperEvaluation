using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DeveloperStore.Sales.Infrastructure.Data
{
    // Esta classe é usada pelo Entity Framework Core Tools em tempo de design
    // para criar uma instância do DbContext quando você executa comandos como Add-Migration ou Update-Database.
    public class SalesDbContextFactory : IDesignTimeDbContextFactory<SalesDbContext>
    {
        public SalesDbContext CreateDbContext(string[] args)
        {
            // Constrói a configuração para ler a string de conexão
            // Isso simula como a string de conexão seria lida em uma aplicação real
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "DeveloperStore.Sales.Api")) // Navega para a pasta do projeto da API
                .AddJsonFile("appsettings.json") // Carrega o appsettings.json da API
                .Build();

            // Obtém a string de conexão
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura as opções do DbContext usando a string de conexão
            var optionsBuilder = new DbContextOptionsBuilder<SalesDbContext>();
            // Você pode escolher o provedor de banco de dados aqui (e.g., UseSqlServer, UseSqlite)
            // Usaremos SQL Server como exemplo, mas você pode mudar para o que estiver usando.
            optionsBuilder.UseSqlServer(connectionString);

            // Retorna uma nova instância do SalesDbContext com as opções configuradas
            return new SalesDbContext(optionsBuilder.Options);
        }
    }
}