using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaReadDto>>> GetAll()
    {
        var categorias = await _categoriaService.GetAllAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaReadDto>> GetById(string id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);

        if (categoria is null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaReadDto>> Create([FromBody] CategoriaCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos de la categoria son obligatorios.");
        }

        try
        {
            var categoriaCreada = await _categoriaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = categoriaCreada.Id }, categoriaCreada);
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
            return Problem("Ocurrio un error al registrar la categoria.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CategoriaUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos de la categoria son obligatorios.");
        }

        try
        {
            var categoriaActualizada = await _categoriaService.UpdateAsync(id, dto);

            if (!categoriaActualizada)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return Problem("Ocurrio un error al actualizar la categoria.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var categoriaEliminada = await _categoriaService.DeleteAsync(id);

            if (!categoriaEliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar la categoria.");
        }
    }
}
