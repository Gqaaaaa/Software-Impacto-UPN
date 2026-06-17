using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IClienteService
{
    Task<List<ClienteReadDto>> GetAllAsync();
    Task<ClienteReadDto?> GetByIdAsync(string id);
    Task<ClienteReadDto> CreateAsync(ClienteCreateDto dto);
    Task<bool> UpdateAsync(string id, ClienteUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
