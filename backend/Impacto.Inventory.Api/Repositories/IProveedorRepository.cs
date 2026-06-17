using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IProveedorRepository
{
    Task<List<Proveedor>> GetAllAsync();
    Task<Proveedor?> GetByIdAsync(string id);
    Task<Proveedor> CreateAsync(Proveedor proveedor);
    Task<bool> UpdateAsync(Proveedor proveedor);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> RucExistsAsync(string ruc, string? excludeId = null);
}
