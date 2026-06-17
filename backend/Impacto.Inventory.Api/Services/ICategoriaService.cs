using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface ICategoriaService
{
    Task<List<CategoriaReadDto>> GetAllAsync();
    Task<CategoriaReadDto?> GetByIdAsync(string id);
    Task<CategoriaReadDto> CreateAsync(CategoriaCreateDto dto);
    Task<bool> UpdateAsync(string id, CategoriaUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
