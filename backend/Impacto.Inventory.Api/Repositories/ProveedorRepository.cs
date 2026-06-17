using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly ImpactoDbContext _context;

    public ProveedorRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Proveedor>> GetAllAsync()
    {
        return await _context.Proveedores
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Proveedor?> GetByIdAsync(string id)
    {
        return await _context.Proveedores
            .AsNoTracking()
            .FirstOrDefaultAsync(proveedor => proveedor.Id == id);
    }

    public async Task<Proveedor> CreateAsync(Proveedor proveedor)
    {
        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();
        return proveedor;
    }

    public async Task<bool> UpdateAsync(Proveedor proveedor)
    {
        var proveedorExistente = await _context.Proveedores
            .FirstOrDefaultAsync(item => item.Id == proveedor.Id);

        if (proveedorExistente is null)
        {
            return false;
        }

        proveedorExistente.RazonSocial = proveedor.RazonSocial;
        proveedorExistente.Ruc = proveedor.Ruc;
        proveedorExistente.Telefono = proveedor.Telefono;
        proveedorExistente.Direccion = proveedor.Direccion;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(item => item.Id == id);

        if (proveedor is null)
        {
            return false;
        }

        _context.Proveedores.Remove(proveedor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Proveedores
            .AnyAsync(proveedor => proveedor.Id == id);
    }

    public async Task<bool> RucExistsAsync(string ruc, string? excludeId = null)
    {
        return await _context.Proveedores
            .AnyAsync(proveedor => proveedor.Ruc == ruc && (excludeId == null || proveedor.Id != excludeId));
    }
}
