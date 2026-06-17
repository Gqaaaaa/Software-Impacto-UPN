using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProductoRepository _productoRepository;

    public VentaService(
        IVentaRepository ventaRepository,
        IClienteRepository clienteRepository,
        IUsuarioRepository usuarioRepository,
        IProductoRepository productoRepository)
    {
        _ventaRepository = ventaRepository;
        _clienteRepository = clienteRepository;
        _usuarioRepository = usuarioRepository;
        _productoRepository = productoRepository;
    }

    public async Task<List<VentaReadDto>> GetAllAsync()
    {
        var ventas = await _ventaRepository.GetAllAsync();
        return ventas.Select(ConvertirAReadDto).ToList();
    }

    public async Task<VentaReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var venta = await _ventaRepository.GetByIdAsync(id.Trim());
        return venta is null ? null : ConvertirAReadDto(venta);
    }

    public async Task<VentaReadDto> CreateAsync(VentaCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var venta = new Venta
        {
            Id = dto.Id.Trim(),
            Fecha = dto.Fecha,
            TipoDocumento = LimpiarTexto(dto.TipoDocumento),
            NumDocumento = LimpiarTexto(dto.NumDocumento),
            Igv = dto.Igv,
            Estado = LimpiarTexto(dto.Estado),
            IdCliente = LimpiarTexto(dto.IdCliente),
            IdUsuario = LimpiarTexto(dto.IdUsuario),
            Detalles = dto.Detalles.Select(detalle => new DetalleVenta
            {
                Id = detalle.Id.Trim(),
                IdVenta = dto.Id.Trim(),
                IdProducto = LimpiarTexto(detalle.IdProducto),
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio
            }).ToList()
        };

        var cantidadesPorProducto = ObtenerCantidadesPorProducto(dto.Detalles);
        await ValidarStockSuficiente(cantidadesPorProducto);

        var ventaCreada = await _ventaRepository.CreateWithStockDecreaseAsync(venta, cantidadesPorProducto);
        var ventaConRelaciones = await _ventaRepository.GetByIdAsync(ventaCreada.Id);
        return ConvertirAReadDto(ventaConRelaciones ?? ventaCreada);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();

        if (!await _ventaRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        return await _ventaRepository.DeleteAsync(idLimpio);
    }

    private async Task ValidarCreateDto(VentaCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id de la venta es obligatorio.");
        }

        if (await _ventaRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe una venta con el id indicado.");
        }

        ValidarLongitud(dto.TipoDocumento, 20, "El tipo de documento no puede superar 20 caracteres.");
        ValidarLongitud(dto.NumDocumento, 20, "El numero de documento no puede superar 20 caracteres.");
        ValidarLongitud(dto.Estado, 20, "El estado no puede superar 20 caracteres.");
        ValidarLongitud(dto.IdCliente, 100, "El id del cliente no puede superar 100 caracteres.");
        ValidarLongitud(dto.IdUsuario, 100, "El id del usuario no puede superar 100 caracteres.");

        if (dto.Igv < 0)
        {
            throw new ArgumentException("El IGV no puede ser negativo.");
        }

        if (!string.IsNullOrWhiteSpace(dto.IdCliente) && !await _clienteRepository.ExistsAsync(dto.IdCliente.Trim()))
        {
            throw new ArgumentException("El cliente indicado no existe.");
        }

        if (!string.IsNullOrWhiteSpace(dto.IdUsuario) && !await _usuarioRepository.ExistsAsync(dto.IdUsuario.Trim()))
        {
            throw new ArgumentException("El usuario indicado no existe.");
        }

        if (dto.Detalles.Count == 0)
        {
            throw new ArgumentException("La venta debe tener al menos un detalle.");
        }

        await ValidarDetalles(dto.Detalles);
    }

    private async Task ValidarDetalles(List<DetalleVentaCreateDto> detalles)
    {
        var idsDetalles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var detalle in detalles)
        {
            if (string.IsNullOrWhiteSpace(detalle.Id))
            {
                throw new ArgumentException("El id del detalle de venta es obligatorio.");
            }

            if (!idsDetalles.Add(detalle.Id.Trim()))
            {
                throw new ArgumentException("No se deben repetir ids en el detalle de venta.");
            }

            if (await _ventaRepository.DetalleExistsAsync(detalle.Id.Trim()))
            {
                throw new InvalidOperationException("Ya existe un detalle de venta con el id indicado.");
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

    private static VentaReadDto ConvertirAReadDto(Venta venta)
    {
        return new VentaReadDto
        {
            Id = venta.Id,
            Fecha = venta.Fecha,
            TipoDocumento = venta.TipoDocumento,
            NumDocumento = venta.NumDocumento,
            Igv = venta.Igv,
            Estado = venta.Estado,
            IdCliente = venta.IdCliente,
            Cliente = CrearNombreCliente(venta.Cliente),
            IdUsuario = venta.IdUsuario,
            Usuario = venta.Usuario?.NombreUsuario,
            Detalles = venta.Detalles.Select(detalle => new DetalleVentaReadDto
            {
                Id = detalle.Id,
                IdProducto = detalle.IdProducto,
                Producto = detalle.Producto?.Nombre,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio
            }).ToList()
        };
    }

    private static string? CrearNombreCliente(Cliente? cliente)
    {
        if (cliente is null)
        {
            return null;
        }

        return string.Join(" ", new[] { cliente.Nombres, cliente.Apellidos }
            .Where(valor => !string.IsNullOrWhiteSpace(valor)));
    }

    private static string? LimpiarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    private async Task ValidarStockSuficiente(Dictionary<string, int> cantidadesPorProducto)
    {
        foreach (var item in cantidadesPorProducto)
        {
            var producto = await _productoRepository.GetByIdAsync(item.Key);

            if (producto is null)
            {
                throw new ArgumentException("Uno de los productos indicados no existe.");
            }

            var stockActual = producto.Stock ?? 0;

            if (stockActual < item.Value)
            {
                throw new InvalidOperationException($"No hay stock suficiente para el producto {producto.Nombre ?? producto.Id}.");
            }
        }
    }

    private static Dictionary<string, int> ObtenerCantidadesPorProducto(List<DetalleVentaCreateDto> detalles)
    {
        return detalles
            .Where(detalle => !string.IsNullOrWhiteSpace(detalle.IdProducto) && detalle.Cantidad.HasValue)
            .GroupBy(detalle => detalle.IdProducto!.Trim())
            .ToDictionary(grupo => grupo.Key, grupo => grupo.Sum(detalle => detalle.Cantidad!.Value));
    }

    private static void ValidarLongitud(string? valor, int maximo, string mensaje)
    {
        if (!string.IsNullOrWhiteSpace(valor) && valor.Trim().Length > maximo)
        {
            throw new ArgumentException(mensaje);
        }
    }
}
