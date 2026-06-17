using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IEmpleadoService
{
    Task<List<EmpleadoReadDto>> GetAllAsync();
    Task<EmpleadoReadDto?> GetByIdAsync(string id);
    Task<EmpleadoReadDto> CreateAsync(EmpleadoCreateDto dto);
    Task<bool> UpdateAsync(string id, EmpleadoUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
