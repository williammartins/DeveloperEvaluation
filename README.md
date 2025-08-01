# DeveloperStore Sales API

Este projeto implementa uma API RESTful completa (CRUD) para gerenciar registros de vendas, seguindo os princípios de Domain-Driven Design (DDD) e utilizando o .NET 8 com Entity Framework Core.

## Sumário

  * [Visão Geral](https://www.google.com/search?q=%23vis%C3%A3o-geral)
  * [Funcionalidades Implementadas](https://www.google.com/search?q=%23funcionalidades-implementadas)
  * [Regras de Negócio](https://www.google.com/search?q=%23regras-de-neg%C3%B3cio)
  * [Pré-requisitos](https://www.google.com/search?q=%23pr%C3%A9-requisitos)
  * [Estrutura do Projeto](https://www.google.com/search?q=%23estrutura-do-projeto)
  * [Configuração e Execução](https://www.google.com/search?q=%23configura%C3%A7%C3%A3o-e-execu%C3%A7%C3%A3o)
  * [Endpoints da API](https://www.google.com/search?q=%23endpoints-da-api)
  * [Testando a API](https://www.google.com/search?q=%23testando-a-api)

-----

## Visão Geral

Esta API de vendas foi desenvolvida para o time da DeveloperStore, com foco em:

  * **DDD (Domain-Driven Design):** Estrutura de código que separa claramente as responsabilidades de domínio, aplicação, infraestrutura e apresentação.
  * **Identidades Externas:** Utilização do padrão para referenciar entidades de outros domínios (Cliente, Filial, Produto) por seus `Guid`s, com a descrição desnormalizada (implícita nos DTOs ou manipulada pelo client).
  * **CRUD Completo:** Suporte total para Criação, Leitura, Atualização e Cancelamento de registros de vendas.

-----

## Funcionalidades Implementadas

A API é capaz de gerenciar os seguintes dados para cada registro de venda:

  * Número da Venda
  * Data da Venda
  * ID do Cliente (Identidade Externa)
  * Valor Total da Venda
  * ID da Filial (Identidade Externa)
  * Produtos vendidos (com ID do Produto - Identidade Externa)
  * Quantidades
  * Preços Unitários
  * Descontos aplicados (por item)
  * Valor Total para cada item
  * Status de Cancelado/Não Cancelado (para a venda e para itens individuais)

-----

## Regras de Negócio

As seguintes regras de negócio relacionadas a descontos por quantidade são aplicadas aos itens da venda:

  * **Compras acima de 4 itens idênticos (4 a 9 itens):** Têm um desconto de **10%**.
  * **Compras entre 10 e 20 itens idênticos (10 a 20 itens):** Têm um desconto de **20%**.
  * **Limite Máximo:** Não é possível vender acima de **20 itens idênticos** por produto em um único item de venda.
  * **Sem Desconto:** Compras **abaixo de 4 itens** idênticos não recebem nenhum desconto.

-----

## Pré-requisitos

Para configurar e executar este projeto em sua máquina, você precisará ter o seguinte instalado:

  * **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**
  * **Visual Studio 2022** (ou uma IDE compatível como VS Code com extensões C\#).
      * **SQL Server LocalDB:** Geralmente instalado junto com o Visual Studio (cargas de trabalho de desenvolvimento web). Se você não tiver, pode instalá-lo como parte do SQL Server Express.
  * **SQL Server Management Studio (SSMS)** ou **Azure Data Studio** (recomendado para gerenciar o banco de dados).

-----

## Estrutura do Projeto

A solução está organizada em quatro projetos, seguindo uma arquitetura inspirada em DDD (Domain-Driven Design):

  * **`DeveloperStore.Sales.Api`**:
      * Projeto ASP.NET Core Web API.
      * Contém os controladores (`SalesController`) que expõem os endpoints RESTful.
      * Configurações de inicialização (`Program.cs`), injeção de dependência e middleware (Swagger/OpenAPI).
  * **`DeveloperStore.Sales.Application`**:
      * Biblioteca de classes contendo os **Data Transfer Objects (DTOs)** para entrada e saída de dados.
      * Define e implementa os **Serviços de Aplicação** (`SaleService`), que orquestram os casos de uso, interagem com o domínio e os repositórios.
  * **`DeveloperStore.Sales.Domain`**:
      * O coração do domínio da aplicação.
      * Contém as **Entidades de Domínio** (`Sale`, `SaleItem`), **Objetos de Valor** (`Money`, `Quantity`).
      * Encapsula as **regras de negócio** e comportamentos do domínio.
      * Define as **Interfaces de Repositório** (`ISaleRepository`).
  * **`DeveloperStore.Sales.Infrastructure`**:
      * Lida com os detalhes técnicos de persistência de dados.
      * Implementa os repositórios (`SaleRepository`) usando **Entity Framework Core**.
      * Contém o **DbContext** (`SalesDbContext`) para mapeamento objeto-relacional.
      * Responsável pelas **Migrações** do banco de dados.

-----

## Configuração e Execução

Siga os passos abaixo para colocar o projeto em funcionamento:

1.  **Clonar o Repositório:**

    ```bash
    git clone [URL_DO_SEU_REPOSITORIO]
    cd DeveloperEvaluation
    ```

2.  **Restaurar Pacotes NuGet:**
    Abra a solução (`DeveloperEvaluation.sln`) no Visual Studio. Os pacotes NuGet devem ser restaurados automaticamente. Se não, clique com o botão direito na solução no Solution Explorer e selecione "Restore NuGet Packages".

3.  **Configurar o Banco de Dados:**
    Este projeto usa SQL Server LocalDB por padrão.

      * **Crie o Banco de Dados:** A conta de usuário que roda a aplicação (sua conta Windows) precisa ter permissão para acessar o banco de dados. É recomendado **criar o banco de dados manualmente** no SQL Server antes de aplicar as migrações:

          * Abra o SQL Server Management Studio (SSMS) ou Azure Data Studio.
          * Conecte-se à sua instância do SQL Server LocalDB (geralmente `(localdb)\mssqllocaldb`).
          * Clique com o botão direito em **"Databases"** e selecione **"New Database..."**.
          * No campo "Database name", digite `DeveloperStoreSalesDb` e clique em OK.

      * **Aplicar Migrações:**

        1.  No Visual Studio, vá em `Tools` \> `NuGet Package Manager` \> `Package Manager Console`.
        2.  No dropdown "Default project", selecione **`DeveloperStore.Sales.Infrastructure`**.
        3.  Execute o comando para aplicar as migrações e criar as tabelas:
            ```powershell
            Update-Database -Context SalesDbContext
            ```
            *Isso irá criar as tabelas `Sales` e `SaleItems` no banco de dados `DeveloperStoreSalesDb`.*

      * **Configuração de String de Conexão (Para SQL Server com Autenticação SQL Server - Opcional):**
        A string de conexão padrão no `DeveloperStore.Sales.Api/appsettings.json` usa autenticação do Windows e LocalDB. Se você precisar usar um usuário e senha de SQL Server específicos (ex: usuário `sa`), **não os adicione diretamente ao `appsettings.json` antes de subir para o Git**. Em vez disso, use o User Secrets Manager para desenvolvimento local:

        1.  Clique com o botão direito no projeto `DeveloperStore.Sales.Api` no Solution Explorer.
        2.  Selecione "Manage User Secrets".
        3.  No arquivo `secrets.json` que será aberto, adicione a sua string de conexão:
            ```json
            {
              "ConnectionStrings": {
                "DefaultConnection": "Server=SEU_SERVIDOR_SQL;Database=DeveloperStoreSalesDb;User ID=seu_usuario;Password=sua_senha_secreta;MultipleActiveResultSets=true;TrustServerCertificate=True"
              }
            }
            ```

4.  **Compilar a Solução:**
    No Visual Studio, vá em `Build` \> `Rebuild Solution` para garantir que todos os projetos sejam compilados com sucesso.

5.  **Executar a Aplicação:**

      * Defina o projeto `DeveloperStore.Sales.Api` como o projeto de inicialização (Startup Project) na sua solução (clique com o botão direito no projeto \> "Set as Startup Project").
      * Pressione `F5` ou clique no botão `Run` (geralmente um triângulo verde) no Visual Studio.
      * A API será iniciada e o navegador deve abrir automaticamente a página do Swagger UI.

-----

## Endpoints da API

A API expõe os seguintes endpoints via `SalesController`:

### Base URL: `https://localhost:XXXXX/api/Sales` (onde `XXXXX` é a porta aleatória, ex: `7080`)

| Método | Endpoint | Descrição | Corpo da Requisição (Exemplo) | Resposta (Exemplo) |
| :----- | :------- | :-------- | :------------------------------ | :----------------- |
| `POST` | `/` | Cria uma nova venda. | Ver [Exemplo de Criação](https://www.google.com/search?q=%23exemplo-de-cria%C3%A7%C3%A3o-de-venda) | `201 Created` + Objeto `SaleResponseDTO` |
| `GET`  | `/{id:guid}` | Obtém uma venda por ID. | (Nenhum) | `200 OK` + Objeto `SaleResponseDTO` ou `404 Not Found` |
| `GET`  | `/` | Obtém todas as vendas. | (Nenhum) | `200 OK` + Array de `SaleResponseDTO` |
| `PUT`  | `/{id:guid}` | Atualiza uma venda existente (permite modificação de itens). | Ver [Exemplo de Atualização](https://www.google.com/search?q=%23exemplo-de-atualiza%C3%A7%C3%A3o-de-venda) | `200 OK` + Objeto `SaleResponseDTO` ou `400 Bad Request` / `404 Not Found` |
| `PATCH`| `/{id:guid}/cancel` | Cancela uma venda. | (Nenhum) | `204 No Content` ou `400 Bad Request` / `404 Not Found` |
| `PATCH`| `/{saleId:guid}/items/{productId:guid}/cancel` | Cancela um item específico de uma venda. | (Nenhum) | `204 No Content` ou `400 Bad Request` / `404 Not Found` |

### Exemplos de DTOs

#### `CreateSaleDTO` (para `POST /api/Sales` e `PUT /api/Sales/{id}`)

```json
{
  "saleNumber": 1001,
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "items": [
    {
      "productId": "1fa85f64-5717-4562-b3fc-2c963f66afa1",
      "quantity": 5,
      "unitPrice": 10.00
    },
    {
      "productId": "2fa85f64-5717-4562-b3fc-2c963f66afa2",
      "quantity": 10,
      "unitPrice": 5.00
    }
  ]
}
```

#### `SaleResponseDTO` (Exemplo de Resposta)

```json
{
  "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "saleNumber": 1001,
  "saleDate": "2025-08-01T15:30:00Z",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "totalAmount": 95.00,
  "isCancelled": false,
  "items": [
    {
      "id": "b1c2d3e4-f5a6-7890-1234-567890abcdef",
      "productId": "1fa85f64-5717-4562-b3fc-2c963f66afa1",
      "quantity": 5,
      "unitPrice": 10.00,
      "discount": 5.00,
      "totalItemAmount": 45.00,
      "isCancelled": false
    },
    {
      "id": "c1d2e3f4-a5b6-7890-1234-567890abcdef",
      "productId": "2fa85f64-5717-4562-b3fc-2c963f66afa2",
      "quantity": 10,
      "unitPrice": 5.00,
      "discount": 10.00,
      "totalItemAmount": 40.00,
      "isCancelled": false
    }
  ]
}
```

-----

## Testando a API

Após executar a aplicação (passo 5 em [Configuração e Execução](https://www.google.com/search?q=%23configura%C3%A7%C3%A3o-e-execu%C3%A7%C3%A3o)), você pode testar a API de duas maneiras:

1.  **Swagger UI (Recomendado para Teste Rápido):**

      * Acesse a URL `https://localhost:XXXXX/swagger` (substitua `XXXXX` pela porta da sua aplicação, visível na saída do console ou no navegador).
      * A interface do Swagger permitirá que você visualize todos os endpoints, modelos de dados, e execute requisições diretamente do navegador.

2.  **Postman, Insomnia ou Ferramenta Similar:**

      * Você pode importar a especificação OpenAPI da sua API (disponível em `https://localhost:XXXXX/swagger/v1/swagger.json`) para essas ferramentas.
      * Crie requisições HTTP para os endpoints listados acima, enviando os `DTOs` no formato JSON conforme os exemplos.
      * Verifique os códigos de status HTTP e o corpo das respostas para confirmar o comportamento esperado.

-----
