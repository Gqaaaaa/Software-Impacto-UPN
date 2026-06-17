using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IVentaService
{
    Task<List<VentaReadDto>> GetAllAsync();
    Task<VentaReadDto?> GetByIdAsync(string id);
    Task<VentaReadDto> CreateAsync(VentaCreateDto dto);
    Task<bool> DeleteAsync(string id);
}
