using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/compras")]
public class ComprasController : ControllerBase
{
    private readonly ICompraService _compraService;

    public ComprasController(ICompraService compraService)
    {
        _compraService = compraService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CompraReadDto>>> GetAll()
    {
        return Ok(await _compraService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompraReadDto>> GetById(string id)
    {
        var compra = await _compraService.GetByIdAsync(id);
        return compra is null ? NotFound() : Ok(compra);
    }

    [HttpPost]
    public async Task<ActionResult<CompraReadDto>> Create([FromBody] CompraCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos de la compra son obligatorios.");
        }

        try
        {
            var compraCreada = await _compraService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = compraCreada.Id }, compraCreada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return Problem("Ocurrio un error al registrar la compra.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _compraService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar la compra.");
        }
    }
}
