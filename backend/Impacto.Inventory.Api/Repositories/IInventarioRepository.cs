using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IInventarioRepository
{
    Task<List<Producto>> GetAllAsync();
}
