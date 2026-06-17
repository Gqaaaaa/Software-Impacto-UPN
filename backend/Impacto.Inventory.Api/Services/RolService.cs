using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;

    public RolService(IRolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    public async Task<List<RolReadDto>> GetAllAsync()
    {
        var roles = await _rolRepository.GetAllAsync();
        return roles.Select(ConvertirAReadDto).ToList();
    }

    public async Task<RolReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var rol = await _rolRepository.GetByIdAsync(id.Trim());
        return rol is null ? null : ConvertirAReadDto(rol);
    }

    public async Task<RolReadDto> CreateAsync(RolCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var rol = new Rol
        {
            Id = dto.Id.Trim(),
            Nombre = LimpiarTexto(dto.Nombre)
        };

        var rolCreado = await _rolRepository.CreateAsync(rol);
        return ConvertirAReadDto(rolCreado);
    }

    public async Task<bool> UpdateAsync(string id, RolUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();
        await ValidarUpdateDto(idLimpio, dto);

        if (!await _rolRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        var rol = new Rol
        {
            Id = idLimpio,
            Nombre = LimpiarTexto(dto.Nombre)
        };

        return await _rolRepository.UpdateAsync(rol);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();

        if (!await _rolRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        return await _rolRepository.DeleteAsync(idLimpio);
    }

    private async Task ValidarCreateDto(RolCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id del rol es obligatorio.");
        }

        if (await _rolRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe un rol con el id indicado.");
        }

        await ValidarDatosRol(dto.Nombre);
    }

    private async Task ValidarUpdateDto(string id, RolUpdateDto dto)
    {
        await ValidarDatosRol(dto.Nombre, id);
    }

    private async Task ValidarDatosRol(string? nombre, string? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del rol es obligatorio.");
        }

        if (nombre.Trim().Length > 50)
        {
            throw new ArgumentException("El nombre del rol no puede superar 50 caracteres.");
        }

        if (await _rolRepository.NombreExistsAsync(nombre.Trim(), excludeId))
        {
            throw new InvalidOperationException("Ya existe un rol con el nombre indicado.");
        }
    }

    private static RolReadDto ConvertirAReadDto(Rol rol)
    {
        return new RolReadDto
        {
            Id = rol.Id,
            Nombre = rol.Nombre
        };
    }

    private static string? LimpiarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
