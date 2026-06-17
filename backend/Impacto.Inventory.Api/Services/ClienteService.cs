using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<List<ClienteReadDto>> GetAllAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        return clientes.Select(ConvertirAReadDto).ToList();
    }

    public async Task<ClienteReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var cliente = await _clienteRepository.GetByIdAsync(id.Trim());
        return cliente is null ? null : ConvertirAReadDto(cliente);
    }

    public async Task<ClienteReadDto> CreateAsync(ClienteCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var cliente = new Cliente
        {
            Id = dto.Id.Trim(),
            Nombres = LimpiarTexto(dto.Nombres),
            Apellidos = LimpiarTexto(dto.Apellidos),
            Dni = LimpiarTexto(dto.Dni),
            Telefono = LimpiarTexto(dto.Telefono),
            Direccion = LimpiarTexto(dto.Direccion)
        };

        var clienteCreado = await _clienteRepository.CreateAsync(cliente);
        return ConvertirAReadDto(clienteCreado);
    }

    public async Task<bool> UpdateAsync(string id, ClienteUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        await ValidarUpdateDto(id.Trim(), dto);

        var existeCliente = await _clienteRepository.ExistsAsync(id.Trim());

        if (!existeCliente)
        {
            return false;
        }

        var cliente = new Cliente
        {
            Id = id.Trim(),
            Nombres = LimpiarTexto(dto.Nombres),
            Apellidos = LimpiarTexto(dto.Apellidos),
            Dni = LimpiarTexto(dto.Dni),
            Telefono = LimpiarTexto(dto.Telefono),
            Direccion = LimpiarTexto(dto.Direccion)
        };

        return await _clienteRepository.UpdateAsync(cliente);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var existeCliente = await _clienteRepository.ExistsAsync(id.Trim());

        if (!existeCliente)
        {
            return false;
        }

        return await _clienteRepository.DeleteAsync(id.Trim());
    }

    private async Task ValidarCreateDto(ClienteCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id del cliente es obligatorio.");
        }

        if (await _clienteRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe un cliente con el id indicado.");
        }

        await ValidarDatosCliente(dto.Dni, dto.Telefono, dto.Nombres, dto.Apellidos, dto.Direccion);
    }

    private async Task ValidarUpdateDto(string id, ClienteUpdateDto dto)
    {
        await ValidarDatosCliente(dto.Dni, dto.Telefono, dto.Nombres, dto.Apellidos, dto.Direccion, id);
    }

    private async Task ValidarDatosCliente(string? dni, string? telefono, string? nombres, string? apellidos, string? direccion, string? excludeId = null)
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

            if (await _clienteRepository.DniExistsAsync(dni.Trim(), excludeId))
            {
                throw new InvalidOperationException("Ya existe un cliente con el DNI indicado.");
            }
        }
    }

    private static ClienteReadDto ConvertirAReadDto(Cliente cliente)
    {
        return new ClienteReadDto
        {
            Id = cliente.Id,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Dni = cliente.Dni,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion
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
