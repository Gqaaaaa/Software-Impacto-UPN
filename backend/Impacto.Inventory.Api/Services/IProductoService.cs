using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IProductoService
{
    Task<List<ProductoReadDto>> GetAllAsync();
    Task<ProductoReadDto?> GetByIdAsync(string id);
    Task<ProductoReadDto> CreateAsync(ProductoCreateDto dto);
    Task<bool> UpdateAsync(string id, ProductoUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
