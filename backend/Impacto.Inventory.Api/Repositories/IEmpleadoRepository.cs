using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IEmpleadoRepository
{
    Task<List<Empleado>> GetAllAsync();
    Task<Empleado?> GetByIdAsync(string id);
    Task<Empleado> CreateAsync(Empleado empleado);
    Task<bool> UpdateAsync(Empleado empleado);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> DniExistsAsync(string dni, string? excludeId = null);
}
