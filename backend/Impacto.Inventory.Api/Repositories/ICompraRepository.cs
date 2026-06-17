using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface ICompraRepository
{
    Task<List<Compra>> GetAllAsync();
    Task<Compra?> GetByIdAsync(string id);
    Task<Compra> CreateAsync(Compra compra);
    Task<Compra> CreateWithStockIncreaseAsync(Compra compra, Dictionary<string, int> cantidadesPorProducto);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> DetalleExistsAsync(string id);
}
