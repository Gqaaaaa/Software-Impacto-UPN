using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly IEmpleadoRepository _empleadoRepository;

    public EmpleadoService(IEmpleadoRepository empleadoRepository)
    {
        _empleadoRepository = empleadoRepository;
    }

    public async Task<List<EmpleadoReadDto>> GetAllAsync()
    {
        var empleados = await _empleadoRepository.GetAllAsync();
        return empleados.Select(ConvertirAReadDto).ToList();
    }

    public async Task<EmpleadoReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var empleado = await _empleadoRepository.GetByIdAsync(id.Trim());
        return empleado is null ? null : ConvertirAReadDto(empleado);
    }

    public async Task<EmpleadoReadDto> CreateAsync(EmpleadoCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var empleado = new Empleado
        {
            Id = dto.Id.Trim(),
            Nombres = LimpiarTexto(dto.Nombres),
            Apellidos = LimpiarTexto(dto.Apellidos),
            Dni = LimpiarTexto(dto.Dni),
            Telefono = LimpiarTexto(dto.Telefono),
            Direccion = LimpiarTexto(dto.Direccion)
        };

        var empleadoCreado = await _empleadoRepository.CreateAsync(empleado);
        return ConvertirAReadDto(empleadoCreado);
    }

    public async Task<bool> UpdateAsync(string id, EmpleadoUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();
        await ValidarUpdateDto(idLimpio, dto);

        if (!await _empleadoRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        var empleado = new Empleado
        {
            Id = idLimpio,
            Nombres = LimpiarTexto(dto.Nombres),
            Apellidos = LimpiarTexto(dto.Apellidos),
            Dni = LimpiarTexto(dto.Dni),
            Telefono = LimpiarTexto(dto.Telefono),
            Direccion = LimpiarTexto(dto.Direccion)
        };

        return await _empleadoRepository.UpdateAsync(empleado);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();

        if (!await _empleadoRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        return await _empleadoRepository.DeleteAsync(idLimpio);
    }

    private async Task ValidarCreateDto(EmpleadoCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id del empleado es obligatorio.");
        }

        if (await _empleadoRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe un empleado con el id indicado.");
        }

        await ValidarDatosEmpleado(dto.Dni, dto.Telefono, dto.Nombres, dto.Apellidos, dto.Direccion);
    }

    private async Task ValidarUpdateDto(string id, EmpleadoUpdateDto dto)
    {
        await ValidarDatosEmpleado(dto.Dni, dto.Telefono, dto.Nombres, dto.Apellidos, dto.Direccion, id);
    }

    private async Task ValidarDatosEmpleado(string? dni, string? telefono, string? nombres, string? apellidos, string? direccion, string? excludeId = null)
    {
        ValidarLongitud(nombres, 100, "Los nombres no pueden superar 100 caracteres.");
        ValidarLongitud(apellidos, 100, "Los apellidos no pueden superar 100 caracteres.");
        ValidarLongitud(telefono, 15, "El telefono no puede superar 15 caracteres.");
        ValidarLongitud(direccion, 150, "La direccion no puede superar 150 caracteres.");

        if (!string.IsNullOrWhiteSpace(dni))
        {
            if (dni.Trim().Length != 8)
            {
                throw new ArgumentException("El DNI debe tener 8 caracteres.");
            }

            if (await _empleadoRepository.DniExistsAsync(dni.Trim(), excludeId))
            {
                throw new InvalidOperationException("Ya existe un empleado con el DNI indicado.");
            }
        }
    }

    private static EmpleadoReadDto ConvertirAReadDto(Empleado empleado)
    {
        return new EmpleadoReadDto
        {
            Id = empleado.Id,
            Nombres = empleado.Nombres,
            Apellidos = empleado.Apellidos,
            Dni = empleado.Dni,
            Telefono = empleado.Telefono,
            Direccion = empleado.Direccion
        };
    }

    private static string? LimpiarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    private static void ValidarLongitud(string? valor, int maximo, string mensaje)
    {
        if (!string.IsNullOrWhiteSpace(valor) && valor.Trim().Length > maximo)
        {
            throw new ArgumentException(mensaje);
        }
    }
}
