using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class VentaRepository : IVentaRepository
{
    private readonly ImpactoDbContext _context;

    public VentaRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Venta>> GetAllAsync()
    {
        return await _context.Ventas
            .AsNoTracking()
            .Include(venta => venta.Cliente)
            .Include(venta => venta.Usuario)
            .Include(venta => venta.Detalles)
                .ThenInclude(detalle => detalle.Producto)
            .ToListAsync();
    }

    public async Task<Venta?> GetByIdAsync(string id)
    {
        return await _context.Ventas
            .AsNoTracking()
            .Include(venta => venta.Cliente)
            .Include(venta => venta.Usuario)
            .Include(venta => venta.Detalles)
                .ThenInclude(detalle => detalle.Producto)
            .FirstOrDefaultAsync(venta => venta.Id == id);
    }

    public async Task<Venta> CreateAsync(Venta venta)
    {
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        return venta;
    }

    public async Task<Venta> CreateWithStockDecreaseAsync(Venta venta, Dictionary<string, int> cantidadesPorProducto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        _context.Ventas.Add(venta);

        foreach (var item in cantidadesPorProducto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(producto => producto.Id == item.Key);

            if (producto is null)
            {
                throw new InvalidOperationException("Uno de los productos indicados no existe.");
            }

            var stockActual = producto.Stock ?? 0;

            if (stockActual < item.Value)
            {
                throw new InvalidOperationException("No hay stock suficiente para uno de los productos.");
            }

            producto.Stock = stockActual - item.Value;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return venta;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var venta = await _context.Ventas
            .Include(item => item.Detalles)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (venta is null)
        {
            return false;
        }

        _context.DetallesVenta.RemoveRange(venta.Detalles);
        _context.Ventas.Remove(venta);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Ventas
            .AnyAsync(venta => venta.Id == id);
    }

    public async Task<bool> DetalleExistsAsync(string id)
    {
        return await _context.DetallesVenta
            .AnyAsync(detalle => detalle.Id == id);
    }
}
