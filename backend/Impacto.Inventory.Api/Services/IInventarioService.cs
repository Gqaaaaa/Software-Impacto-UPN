using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IInventarioService
{
    Task<List<InventarioReadDto>> GetAllAsync();
}
