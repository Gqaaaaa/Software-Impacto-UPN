using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoReadDto>>> GetAll()
    {
        return Ok(await _productoService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoReadDto>> GetById(string id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoReadDto>> Create([FromBody] ProductoCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del producto son obligatorios.");
        }

        try
        {
            var productoCreado = await _productoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = productoCreado.Id }, productoCreado);
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
            return Problem("Ocurrio un error al registrar el producto.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductoUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del producto son obligatorios.");
        }

        try
        {
            var actualizado = await _productoService.UpdateAsync(id, dto);
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
            return Problem("Ocurrio un error al actualizar el producto.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _productoService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar el producto.");
        }
    }
}
