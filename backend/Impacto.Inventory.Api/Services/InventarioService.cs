using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class InventarioService : IInventarioService
{
    private readonly IInventarioRepository _inventarioRepository;

    public InventarioService(IInventarioRepository inventarioRepository)
    {
        _inventarioRepository = inventarioRepository;
    }

    public async Task<List<InventarioReadDto>> GetAllAsync()
    {
        var productos = await _inventarioRepository.GetAllAsync();
        return productos.Select(ConvertirAReadDto).ToList();
    }

    private static InventarioReadDto ConvertirAReadDto(Producto producto)
    {
        return new InventarioReadDto
        {
            IdProducto = producto.Id,
            NombreProducto = producto.Nombre,
            Categoria = producto.Categoria?.Descripcion,
            StockActual = producto.Stock,
            PrecioCompra = producto.PrecioCompra,
            PrecioVenta = producto.PrecioVenta
        };
    }
}
