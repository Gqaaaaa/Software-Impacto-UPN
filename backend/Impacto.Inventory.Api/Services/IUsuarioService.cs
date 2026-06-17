using Impacto.Inventory.Api.DTOs;

namespace Impacto.Inventory.Api.Services;

public interface IUsuarioService
{
    Task<List<UsuarioReadDto>> GetAllAsync();
    Task<UsuarioReadDto?> GetByIdAsync(string id);
    Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto dto);
    Task<bool> UpdateAsync(string id, UsuarioUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}
