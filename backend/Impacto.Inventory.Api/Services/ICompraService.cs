using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface ICompraService
{
    Task<List<CompraReadDto>> GetAllAsync();
    Task<CompraReadDto?> GetByIdAsync(string id);
    Task<CompraReadDto> CreateAsync(CompraCreateDto dto);
    Task<bool> DeleteAsync(string id);
}
