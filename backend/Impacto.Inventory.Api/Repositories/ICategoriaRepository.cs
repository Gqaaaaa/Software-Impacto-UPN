using Impacto.Inventory.Api.Models;

namespace Impacto.Inventory.Api.Repositories;

public interface ICategoriaRepository
{
    Task<List<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(string id);
    Task<Categoria> CreateAsync(Categoria categoria);
    Task<bool> UpdateAsync(Categoria categoria);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
