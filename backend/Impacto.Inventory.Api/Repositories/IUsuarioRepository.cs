using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IUsuarioRepository
{
    Task<List<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(string id);
    Task<Usuario> CreateAsync(Usuario usuario, string? idRol = null);
    Task<bool> UpdateAsync(Usuario usuario, string? idRol = null);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> NombreUsuarioExistsAsync(string usuario, string? excludeId = null);
}
