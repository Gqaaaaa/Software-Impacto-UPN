using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IVentaRepository
{
    Task<List<Venta>> GetAllAsync();
    Task<Venta?> GetByIdAsync(string id);
    Task<Venta> CreateAsync(Venta venta);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> DetalleExistsAsync(string id);
}
