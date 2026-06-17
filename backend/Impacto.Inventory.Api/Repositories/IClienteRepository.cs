using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface IClienteRepository
{
    Task<List<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(string id);
    Task<Cliente> CreateAsync(Cliente cliente);
    Task<bool> UpdateAsync(Cliente cliente);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<bool> DniExistsAsync(string dni, string? excludeId = null);
}
