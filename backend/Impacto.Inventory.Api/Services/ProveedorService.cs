using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _proveedorRepository;

    public ProveedorService(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<List<ProveedorReadDto>> GetAllAsync()
    {
        var proveedores = await _proveedorRepository.GetAllAsync();
        return proveedores.Select(ConvertirAReadDto).ToList();
    }

    public async Task<ProveedorReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var proveedor = await _proveedorRepository.GetByIdAsync(id.Trim());
        return proveedor is null ? null : ConvertirAReadDto(proveedor);
    }

    public async Task<ProveedorReadDto> CreateAsync(ProveedorCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var proveedor = new Proveedor
        {
            Id = dto.Id.Trim(),
            RazonSocial = LimpiarTexto(dto.RazonSocial),
            Ruc = LimpiarTexto(dto.Ruc),
            Telefono = LimpiarTexto(dto.Telefono),
            Direccion = LimpiarTexto(dto.Direccion)
        };

        var proveedorCreado = await _proveedorRepository.CreateAsync(proveedor);
        return ConvertirAReadDto(proveedorCreado);
    }

    public async Task<bool> UpdateAsync(string id, ProveedorUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        await ValidarUpdateDto(id.Trim(), dto);

        var existeProveedor = await _proveedorRepository.ExistsAsync(id.Trim());

        if (!existeProveedor)
        {
            return false;
        }

        var proveedor = new Proveedor
        {
            Id = id.Trim(),
            RazonSocial = LimpiarTexto(dto.RazonSocial),
            Ruc = LimpiarTexto(dto.Ruc),
            Telefono = LimpiarTexto(dto.Telefono),
            Direccion = LimpiarTexto(dto.Direccion)
        };

        return await _proveedorRepository.UpdateAsync(proveedor);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var existeProveedor = await _proveedorRepository.ExistsAsync(id.Trim());

        if (!existeProveedor)
        {
            return false;
        }

        return await _proveedorRepository.DeleteAsync(id.Trim());
    }

    private async Task ValidarCreateDto(ProveedorCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id del proveedor es obligatorio.");
        }

        if (await _proveedorRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe un proveedor con el id indicado.");
        }

        await ValidarDatosProveedor(dto.Ruc, dto.Telefono, dto.RazonSocial, dto.Direccion);
    }

    private async Task ValidarUpdateDto(string id, ProveedorUpdateDto dto)
    {
        await ValidarDatosProveedor(dto.Ruc, dto.Telefono, dto.RazonSocial, dto.Direccion, id);
    }

    private async Task ValidarDatosProveedor(string? ruc, string? telefono, string? razonSocial, string? direccion, string? excludeId = null)
    {
        ValidarLongitud(razonSocial, 150, "La razon social no puede superar 150 caracteres.");
        ValidarLongitud(telefono, 15, "El telefono no puede superar 15 caracteres.");
        ValidarLongitud(direccion, 150, "La direccion no puede superar 150 caracteres.");

        if (!string.IsNullOrWhiteSpace(ruc))
        {
            if (ruc.Trim().Length != 11)
            {
                throw new ArgumentException("El RUC debe tener 11 caracteres.");
            }

            if (await _proveedorRepository.RucExistsAsync(ruc.Trim(), excludeId))
            {
                throw new InvalidOperationException("Ya existe un proveedor con el RUC indicado.");
            }
        }
    }

    private static ProveedorReadDto ConvertirAReadDto(Proveedor proveedor)
    {
        return new ProveedorReadDto
        {
            Id = proveedor.Id,
            RazonSocial = proveedor.RazonSocial,
            Ruc = proveedor.Ruc,
            Telefono = proveedor.Telefono,
            Direccion = proveedor.Direccion
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
