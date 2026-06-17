using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ImpactoDbContext _context;

    public ClienteRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cliente?> GetByIdAsync(string id)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(cliente => cliente.Id == id);
    }

    public async Task<Cliente> CreateAsync(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> UpdateAsync(Cliente cliente)
    {
        var clienteExistente = await _context.Clientes
            .FirstOrDefaultAsync(item => item.Id == cliente.Id);

        if (clienteExistente is null)
        {
            return false;
        }

        clienteExistente.Nombres = cliente.Nombres;
        clienteExistente.Apellidos = cliente.Apellidos;
        clienteExistente.Dni = cliente.Dni;
        clienteExistente.Telefono = cliente.Telefono;
        clienteExistente.Direccion = cliente.Direccion;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(item => item.Id == id);

        if (cliente is null)
        {
            return false;
        }

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Clientes
            .AnyAsync(cliente => cliente.Id == id);
    }

    public async Task<bool> DniExistsAsync(string dni, string? excludeId = null)
    {
        return await _context.Clientes
            .AnyAsync(cliente => cliente.Dni == dni && (excludeId == null || cliente.Id != excludeId));
    }
}
