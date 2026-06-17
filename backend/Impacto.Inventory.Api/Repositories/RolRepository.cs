using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class RolRepository : IRolRepository
{
    private readonly ImpactoDbContext _context;

    public RolRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rol>> GetAllAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Rol?> GetByIdAsync(string id)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(rol => rol.Id == id);
    }

    public async Task<Rol> CreateAsync(Rol rol)
    {
        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();
        return rol;
    }

    public async Task<bool> UpdateAsync(Rol rol)
    {
        var rolExistente = await _context.Roles
            .FirstOrDefaultAsync(item => item.Id == rol.Id);

        if (rolExistente is null)
        {
            return false;
        }

        rolExistente.Nombre = rol.Nombre;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var rol = await _context.Roles
            .FirstOrDefaultAsync(item => item.Id == id);

        if (rol is null)
        {
            return false;
        }

        _context.Roles.Remove(rol);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Roles
            .AnyAsync(rol => rol.Id == id);
    }

    public async Task<bool> NombreExistsAsync(string nombre, string? excludeId = null)
    {
        return await _context.Roles
            .AnyAsync(rol => rol.Nombre == nombre && (excludeId == null || rol.Id != excludeId));
    }
}
