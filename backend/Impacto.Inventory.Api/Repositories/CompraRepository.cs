using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class CompraRepository : ICompraRepository
{
    private readonly ImpactoDbContext _context;

    public CompraRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Compra>> GetAllAsync()
    {
        return await _context.Compras
            .AsNoTracking()
            .Include(compra => compra.Proveedor)
            .Include(compra => compra.Usuario)
            .Include(compra => compra.Detalles)
                .ThenInclude(detalle => detalle.Producto)
            .ToListAsync();
    }

    public async Task<Compra?> GetByIdAsync(string id)
    {
        return await _context.Compras
            .AsNoTracking()
            .Include(compra => compra.Proveedor)
            .Include(compra => compra.Usuario)
            .Include(compra => compra.Detalles)
                .ThenInclude(detalle => detalle.Producto)
            .FirstOrDefaultAsync(compra => compra.Id == id);
    }

    public async Task<Compra> CreateAsync(Compra compra)
    {
        _context.Compras.Add(compra);
        await _context.SaveChangesAsync();
        return compra;
    }

    public async Task<Compra> CreateWithStockIncreaseAsync(Compra compra, Dictionary<string, int> cantidadesPorProducto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        _context.Compras.Add(compra);

        foreach (var item in cantidadesPorProducto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(producto => producto.Id == item.Key);

            if (producto is null)
            {
                throw new InvalidOperationException("Uno de los productos indicados no existe.");
            }

            producto.Stock = (producto.Stock ?? 0) + item.Value;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return compra;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var compra = await _context.Compras
            .Include(item => item.Detalles)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (compra is null)
        {
            return false;
        }

        _context.DetallesCompra.RemoveRange(compra.Detalles);
        _context.Compras.Remove(compra);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Compras
            .AnyAsync(compra => compra.Id == id);
    }

    public async Task<bool> DetalleExistsAsync(string id)
    {
        return await _context.DetallesCompra
            .AnyAsync(detalle => detalle.Id == id);
    }
}
