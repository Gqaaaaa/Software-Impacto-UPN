using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly ImpactoDbContext _context;

    public EmpleadoRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Empleado>> GetAllAsync()
    {
        return await _context.Empleados
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Empleado?> GetByIdAsync(string id)
    {
        return await _context.Empleados
            .AsNoTracking()
            .FirstOrDefaultAsync(empleado => empleado.Id == id);
    }

    public async Task<Empleado> CreateAsync(Empleado empleado)
    {
        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();
        return empleado;
    }

    public async Task<bool> UpdateAsync(Empleado empleado)
    {
        var empleadoExistente = await _context.Empleados
            .FirstOrDefaultAsync(item => item.Id == empleado.Id);

        if (empleadoExistente is null)
        {
            return false;
        }

        empleadoExistente.Nombres = empleado.Nombres;
        empleadoExistente.Apellidos = empleado.Apellidos;
        empleadoExistente.Dni = empleado.Dni;
        empleadoExistente.Telefono = empleado.Telefono;
        empleadoExistente.Direccion = empleado.Direccion;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(item => item.Id == id);

        if (empleado is null)
        {
            return false;
        }

        _context.Empleados.Remove(empleado);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Empleados
            .AnyAsync(empleado => empleado.Id == id);
    }

    public async Task<bool> DniExistsAsync(string dni, string? excludeId = null)
    {
        return await _context.Empleados
            .AnyAsync(empleado => empleado.Dni == dni && (excludeId == null || empleado.Id != excludeId));
    }
}
