using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IRolService
{
    Task<List<RolReadDto>> GetAllAsync();
    Task<RolReadDto?> GetByIdAsync(string id);
    Task<RolReadDto> CreateAsync(RolCreateDto dto);
    Task<bool> UpdateAsync(string id, RolUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
