using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class InventarioRepository : IInventarioRepository
{
    private readonly ImpactoDbContext _context;

    public InventarioRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> GetAllAsync()
    {
        return await _context.Productos
            .AsNoTracking()
            .Include(producto => producto.Categoria)
            .ToListAsync();
    }
}
