using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impacto.Inventory.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClienteReadDto>>> GetAll()
    {
        return Ok(await _clienteService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteReadDto>> GetById(string id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteReadDto>> Create([FromBody] ClienteCreateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del cliente son obligatorios.");
        }

        try
        {
            var clienteCreado = await _clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = clienteCreado.Id }, clienteCreado);
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
            return Problem("Ocurrio un error al registrar el cliente.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ClienteUpdateDto dto)
    {
        if (dto is null)
        {
            return BadRequest("Los datos del cliente son obligatorios.");
        }

        try
        {
            var actualizado = await _clienteService.UpdateAsync(id, dto);
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
            return Problem("Ocurrio un error al actualizar el cliente.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var eliminado = await _clienteService.DeleteAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch
        {
            return Problem("Ocurrio un error al eliminar el cliente.");
        }
    }
}
