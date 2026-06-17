using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly ImpactoDbContext _context;

    public ProductoRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> GetAllAsync()
    {
        return await _context.Productos
            .Include(producto => producto.Categoria)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Producto?> GetByIdAsync(string id)
    {
        return await _context.Productos
            .Include(producto => producto.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(producto => producto.Id == id);
    }

    public async Task<Producto> CreateAsync(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<bool> UpdateAsync(Producto producto)
    {
        var productoExistente = await _context.Productos
            .FirstOrDefaultAsync(item => item.Id == producto.Id);

        if (productoExistente is null)
        {
            return false;
        }

        productoExistente.Nombre = producto.Nombre;
        productoExistente.Descripcion = producto.Descripcion;
        productoExistente.PrecioCompra = producto.PrecioCompra;
        productoExistente.PrecioVenta = producto.PrecioVenta;
        productoExistente.Stock = producto.Stock;
        productoExistente.IdCategoria = producto.IdCategoria;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(item => item.Id == id);

        if (producto is null)
        {
            return false;
        }

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Productos
            .AnyAsync(producto => producto.Id == id);
    }
}
