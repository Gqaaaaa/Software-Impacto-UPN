using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _compraRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProductoRepository _productoRepository;

    public CompraService(
        ICompraRepository compraRepository,
        IProveedorRepository proveedorRepository,
        IUsuarioRepository usuarioRepository,
        IProductoRepository productoRepository)
    {
        _compraRepository = compraRepository;
        _proveedorRepository = proveedorRepository;
        _usuarioRepository = usuarioRepository;
        _productoRepository = productoRepository;
    }

    public async Task<List<CompraReadDto>> GetAllAsync()
    {
        var compras = await _compraRepository.GetAllAsync();
        return compras.Select(ConvertirAReadDto).ToList();
    }

    public async Task<CompraReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var compra = await _compraRepository.GetByIdAsync(id.Trim());
        return compra is null ? null : ConvertirAReadDto(compra);
    }

    public async Task<CompraReadDto> CreateAsync(CompraCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var compra = new Compra
        {
            Id = dto.Id.Trim(),
            Fecha = dto.Fecha,
            TipoDocumento = LimpiarTexto(dto.TipoDocumento),
            NumDocumento = LimpiarTexto(dto.NumDocumento),
            Igv = dto.Igv,
            Estado = LimpiarTexto(dto.Estado),
            IdProveedor = LimpiarTexto(dto.IdProveedor),
            IdUsuario = LimpiarTexto(dto.IdUsuario),
            Detalles = dto.Detalles.Select(detalle => new DetalleCompra
            {
                Id = detalle.Id.Trim(),
                IdCompra = dto.Id.Trim(),
                IdProducto = LimpiarTexto(detalle.IdProducto),
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio
            }).ToList()
        };

        // Pendiente: el aumento de stock se implementara en el bloque de inventario.
        var compraCreada = await _compraRepository.CreateAsync(compra);
        var compraConRelaciones = await _compraRepository.GetByIdAsync(compraCreada.Id);
        return ConvertirAReadDto(compraConRelaciones ?? compraCreada);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();

        if (!await _compraRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        return await _compraRepository.DeleteAsync(idLimpio);
    }

    private async Task ValidarCreateDto(CompraCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id de la compra es obligatorio.");
        }

        if (await _compraRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe una compra con el id indicado.");
        }

        ValidarLongitud(dto.TipoDocumento, 20, "El tipo de documento no puede superar 20 caracteres.");
        ValidarLongitud(dto.NumDocumento, 20, "El numero de documento no puede superar 20 caracteres.");
        ValidarLongitud(dto.Estado, 20, "El estado no puede superar 20 caracteres.");
        ValidarLongitud(dto.IdProveedor, 100, "El id del proveedor no puede superar 100 caracteres.");
        ValidarLongitud(dto.IdUsuario, 100, "El id del usuario no puede superar 100 caracteres.");

        if (dto.Igv < 0)
        {
            throw new ArgumentException("El IGV no puede ser negativo.");
        }

        if (!string.IsNullOrWhiteSpace(dto.IdProveedor) && !await _proveedorRepository.ExistsAsync(dto.IdProveedor.Trim()))
        {
            throw new ArgumentException("El proveedor indicado no existe.");
        }

        if (!string.IsNullOrWhiteSpace(dto.IdUsuario) && !await _usuarioRepository.ExistsAsync(dto.IdUsuario.Trim()))
        {
            throw new ArgumentException("El usuario indicado no existe.");
        }

        if (dto.Detalles.Count == 0)
        {
            throw new ArgumentException("La compra debe tener al menos un detalle.");
        }

        await ValidarDetalles(dto.Detalles);
    }

    private async Task ValidarDetalles(List<DetalleCompraCreateDto> detalles)
    {
        var idsDetalles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var detalle in detalles)
        {
            if (string.IsNullOrWhiteSpace(detalle.Id))
            {
                throw new ArgumentException("El id del detalle de compra es obligatorio.");
            }

            if (!idsDetalles.Add(detalle.Id.Trim()))
            {
                throw new ArgumentException("No se deben repetir ids en el detalle de compra.");
            }

            if (await _compraRepository.DetalleExistsAsync(detalle.Id.Trim()))
            {
                throw new InvalidOperationException("Ya existe un detalle de compra con el id indicado.");
            }

            if (string.IsNullOrWhiteSpace(detalle.IdProducto))
            {
                throw new ArgumentException("El producto del detalle es obligatorio.");
            }

            ValidarLongitud(detalle.IdProducto, 100, "El id del producto no puede superar 100 caracteres.");

            if (!await _productoRepository.ExistsAsync(detalle.IdProducto.Trim()))
            {
                throw new ArgumentException("Uno de los productos indicados no existe.");
            }

            if (detalle.Cantidad is null || detalle.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor que cero.");
            }

            if (detalle.Precio is null || detalle.Precio < 0)
            {
                throw new ArgumentException("El precio debe ser mayor o igual que cero.");
            }
        }
    }

    private static CompraReadDto ConvertirAReadDto(Compra compra)
    {
        return new CompraReadDto
        {
            Id = compra.Id,
            Fecha = compra.Fecha,
            TipoDocumento = compra.TipoDocumento,
            NumDocumento = compra.NumDocumento,
            Igv = compra.Igv,
            Estado = compra.Estado,
            IdProveedor = compra.IdProveedor,
            Proveedor = compra.Proveedor?.RazonSocial,
            IdUsuario = compra.IdUsuario,
            Usuario = compra.Usuario?.NombreUsuario,
            Detalles = compra.Detalles.Select(detalle => new DetalleCompraReadDto
            {
                Id = detalle.Id,
                IdProducto = detalle.IdProducto,
                Producto = detalle.Producto?.Nombre,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio
            }).ToList()
        };
    }

    private static string? LimpiarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    private static void ValidarLongitud(string? valor, int maximo, string mensaje)
    {
        if (!string.IsNullOrWhiteSpace(valor) && valor.Trim().Length > maximo)
        {
            throw new ArgumentException(mensaje);
        }
    }
}
