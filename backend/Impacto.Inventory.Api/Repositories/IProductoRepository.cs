using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllAsync();
    Task<Producto?> GetByIdAsync(string id);
    Task<Producto> CreateAsync(Producto producto);
    Task<bool> UpdateAsync(Producto producto);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
