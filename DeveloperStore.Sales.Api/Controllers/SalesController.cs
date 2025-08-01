using DeveloperStore.Sales.Application.DTOs;
using DeveloperStore.Sales.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        /// <param name="createSaleDto">Dados para criar a venda.</param>
        /// <returns>A venda criada.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SaleResponseDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDTO createSaleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var sale = await _saleService.CreateSale(createSaleDto);
                return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtém uma venda pelo seu ID.
        /// </summary>
        /// <param name="id">ID da venda.</param>
        /// <returns>A venda encontrada.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SaleResponseDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            var sale = await _saleService.GetSaleById(id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        /// <summary>
        /// Obtém todas as vendas.
        /// </summary>
        /// <returns>Lista de vendas.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleResponseDTO>), 200)]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSales();
            return Ok(sales);
        }

        /// <summary>
        /// Atualiza uma venda existente.
        /// </summary>
        /// <param name="id">ID da venda a ser atualizada.</param>
        /// <param name="updateSaleDto">Dados atualizados da venda.</param>
        /// <returns>A venda atualizada.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(SaleResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] CreateSaleDTO updateSaleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedSale = await _saleService.UpdateSale(id, updateSaleDto);
                return Ok(updatedSale);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancela uma venda.
        /// </summary>
        /// <param name="id">ID da venda a ser cancelada.</param>
        /// <returns>No Content se bem-sucedido.</returns>
        [HttpPatch("{id:guid}/cancel")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            try
            {
                await _saleService.CancelSale(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancela um item específico de uma venda.
        /// </summary>
        /// <param name="saleId">ID da venda.</param>
        /// <param name="productId">ID do produto (item) a ser cancelado.</param>
        /// <returns>No Content se bem-sucedido.</returns>
        [HttpPatch("{saleId:guid}/items/{productId:guid}/cancel")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelSaleItem(Guid saleId, Guid productId)
        {
            try
            {
                await _saleService.CancelSaleItem(saleId, productId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}