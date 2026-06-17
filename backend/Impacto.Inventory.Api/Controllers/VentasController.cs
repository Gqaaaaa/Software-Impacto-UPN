using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/ventas")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;

    public VentasController(IVentaService ventaService)
    {
        _ventaService = ventaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<VentaReadDto>>> GetAll()
    {
        return Ok(await _ventaService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VentaReadDto>> GetById(string id)
    {
        var venta = await _ventaService.GetByIdAsync(id);
        return venta is null ? NotFound() : Ok(venta);
    }

    [HttpPost]
    public async Task<ActionResult<VentaReadDto>> Create([FromBody] VentaCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos de la venta son obligatorios.");
        }

        try
        {
            var ventaCreada = await _ventaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ventaCreada.Id }, ventaCreada);
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
            return Problem("Ocurrio un error al registrar la venta.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _ventaService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar la venta.");
        }
    }
}
