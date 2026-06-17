using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/proveedores")]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorService _proveedorService;

    public ProveedoresController(IProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProveedorReadDto>>> GetAll()
    {
        return Ok(await _proveedorService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProveedorReadDto>> GetById(string id)
    {
        var proveedor = await _proveedorService.GetByIdAsync(id);
        return proveedor is null ? NotFound() : Ok(proveedor);
    }

    [HttpPost]
    public async Task<ActionResult<ProveedorReadDto>> Create([FromBody] ProveedorCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del proveedor son obligatorios.");
        }

        try
        {
            var proveedorCreado = await _proveedorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = proveedorCreado.Id }, proveedorCreado);
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
            return Problem("Ocurrio un error al registrar el proveedor.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProveedorUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del proveedor son obligatorios.");
        }

        try
        {
            var actualizado = await _proveedorService.UpdateAsync(id, dto);
            return actualizado ? NoContent() : NotFound();
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
            return Problem("Ocurrio un error al actualizar el proveedor.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _proveedorService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar el proveedor.");
        }
    }
}
