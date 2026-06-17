using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioReadDto>>> GetAll()
    {
        return Ok(await _usuarioService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioReadDto>> GetById(string id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioReadDto>> Create([FromBody] UsuarioCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del usuario son obligatorios.");
        }

        try
        {
            var usuarioCreado = await _usuarioService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = usuarioCreado.Id }, usuarioCreado);
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
            return Problem("Ocurrio un error al registrar el usuario.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UsuarioUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del usuario son obligatorios.");
        }

        try
        {
            var actualizado = await _usuarioService.UpdateAsync(id, dto);
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
            return Problem("Ocurrio un error al actualizar el usuario.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _usuarioService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar el usuario.");
        }
    }
}
