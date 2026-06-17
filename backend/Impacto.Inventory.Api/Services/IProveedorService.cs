using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IProveedorService
{
    Task<List<ProveedorReadDto>> GetAllAsync();
    Task<ProveedorReadDto?> GetByIdAsync(string id);
    Task<ProveedorReadDto> CreateAsync(ProveedorCreateDto dto);
    Task<bool> UpdateAsync(string id, ProveedorUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
