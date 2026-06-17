using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolesController(IRolService rolService)
    {
        _rolService = rolService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RolReadDto>>> GetAll()
    {
        return Ok(await _rolService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RolReadDto>> GetById(string id)
    {
        var rol = await _rolService.GetByIdAsync(id);
        return rol is null ? NotFound() : Ok(rol);
    }

    [HttpPost]
    public async Task<ActionResult<RolReadDto>> Create([FromBody] RolCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del rol son obligatorios.");
        }

        try
        {
            var rolCreado = await _rolService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = rolCreado.Id }, rolCreado);
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
            return Problem("Ocurrio un error al registrar el rol.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] RolUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del rol son obligatorios.");
        }

        try
        {
            var actualizado = await _rolService.UpdateAsync(id, dto);
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
            return Problem("Ocurrio un error al actualizar el rol.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _rolService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar el rol.");
        }
    }
}
