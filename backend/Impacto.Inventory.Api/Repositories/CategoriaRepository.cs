using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ImpactoDbContext _context;

    public CategoriaRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetAllAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(string id)
    {
        return await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(categoria => categoria.Id == id);
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<bool> UpdateAsync(Categoria categoria)
    {
        var categoriaExistente = await _context.Categorias
            .FirstOrDefaultAsync(item => item.Id == categoria.Id);

        if (categoriaExistente is null)
        {
            return false;
        }

        categoriaExistente.Descripcion = categoria.Descripcion;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(item => item.Id == id);

        if (categoria is null)
        {
            return false;
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Categorias
            .AnyAsync(categoria => categoria.Id == id);
    }
}
