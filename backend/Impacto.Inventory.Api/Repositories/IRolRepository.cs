using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IRolRepository
{
    Task<List<Rol>> GetAllAsync();
    Task<Rol?> GetByIdAsync(string id);
    Task<Rol> CreateAsync(Rol rol);
    Task<bool> UpdateAsync(Rol rol);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> NombreExistsAsync(string nombre, string? excludeId = null);
}
