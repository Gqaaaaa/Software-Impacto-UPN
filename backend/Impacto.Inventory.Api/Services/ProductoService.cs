using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ProductoService(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<ProductoReadDto>> GetAllAsync()
    {
        var productos = await _productoRepository.GetAllAsync();
        return productos.Select(ConvertirAReadDto).ToList();
    }

    public async Task<ProductoReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var producto = await _productoRepository.GetByIdAsync(id.Trim());
        return producto is null ? null : ConvertirAReadDto(producto);
    }

    public async Task<ProductoReadDto> CreateAsync(ProductoCreateDto dto)
    {
        await ValidarProductoCreateDto(dto);

        var producto = new Producto
        {
            Id = dto.Id.Trim(),
            Nombre = dto.Nombre?.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            Stock = dto.Stock,
            IdCategoria = dto.IdCategoria?.Trim()
        };

        var productoCreado = await _productoRepository.CreateAsync(producto);
        return ConvertirAReadDto(productoCreado);
    }

    public async Task<bool> UpdateAsync(string id, ProductoUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        await ValidarProductoUpdateDto(dto);

        var existeProducto = await _productoRepository.ExistsAsync(id.Trim());

        if (!existeProducto)
        {
            return false;
        }

        var producto = new Producto
        {
            Id = id.Trim(),
            Nombre = dto.Nombre?.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            Stock = dto.Stock,
            IdCategoria = dto.IdCategoria?.Trim()
        };

        return await _productoRepository.UpdateAsync(producto);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var existeProducto = await _productoRepository.ExistsAsync(id.Trim());

        if (!existeProducto)
        {
            return false;
        }

        return await _productoRepository.DeleteAsync(id.Trim());
    }

    private static ProductoReadDto ConvertirAReadDto(Producto producto)
    {
        return new ProductoReadDto
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            PrecioCompra = producto.PrecioCompra,
            PrecioVenta = producto.PrecioVenta,
            Stock = producto.Stock,
            IdCategoria = producto.IdCategoria,
            CategoriaDescripcion = producto.Categoria?.Descripcion
        };
    }

    private async Task ValidarProductoCreateDto(ProductoCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id del producto es obligatorio.");
        }

        var existeProducto = await _productoRepository.ExistsAsync(dto.Id.Trim());

        if (existeProducto)
        {
            throw new InvalidOperationException("Ya existe un producto con el id indicado.");
        }

        await ValidarDatosProducto(dto.PrecioCompra, dto.PrecioVenta, dto.Stock, dto.IdCategoria);
    }

    private async Task ValidarProductoUpdateDto(ProductoUpdateDto dto)
    {
        await ValidarDatosProducto(dto.PrecioCompra, dto.PrecioVenta, dto.Stock, dto.IdCategoria);
    }

    private async Task ValidarDatosProducto(decimal? precioCompra, decimal? precioVenta, int? stock, string? idCategoria)
    {
        if (precioCompra.HasValue && precioCompra.Value < 0)
        {
            throw new ArgumentException("El precio de compra no puede ser negativo.");
        }

        if (precioVenta.HasValue && precioVenta.Value < 0)
        {
            throw new ArgumentException("El precio de venta no puede ser negativo.");
        }

        if (stock.HasValue && stock.Value < 0)
        {
            throw new ArgumentException("El stock no puede ser negativo.");
        }

        if (!string.IsNullOrWhiteSpace(idCategoria))
        {
            var existeCategoria = await _categoriaRepository.ExistsAsync(idCategoria.Trim());

            if (!existeCategoria)
            {
                throw new ArgumentException("La categoria indicada no existe.");
            }
        }
    }
}
