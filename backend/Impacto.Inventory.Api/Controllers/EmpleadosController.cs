using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/empleados")]
public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoService _empleadoService;

    public EmpleadosController(IEmpleadoService empleadoService)
    {
        _empleadoService = empleadoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmpleadoReadDto>>> GetAll()
    {
        return Ok(await _empleadoService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmpleadoReadDto>> GetById(string id)
    {
        var empleado = await _empleadoService.GetByIdAsync(id);
        return empleado is null ? NotFound() : Ok(empleado);
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadoReadDto>> Create([FromBody] EmpleadoCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del empleado son obligatorios.");
        }

        try
        {
            var empleadoCreado = await _empleadoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = empleadoCreado.Id }, empleadoCreado);
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
            return Problem("Ocurrio un error al registrar el empleado.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] EmpleadoUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del empleado son obligatorios.");
        }

        try
        {
            var actualizado = await _empleadoService.UpdateAsync(id, dto);
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
            return Problem("Ocurrio un error al actualizar el empleado.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _empleadoService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar el empleado.");
        }
    }
}
