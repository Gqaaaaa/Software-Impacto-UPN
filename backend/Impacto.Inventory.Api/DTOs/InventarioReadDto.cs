namespace Impacto.Inventory.Api.DTOs;

public class InventarioReadDto
{
    public string IdProducto { get; set; } = string.Empty;
    public string? NombreProducto { get; set; }
    public string? Categoria { get; set; }
    public int? StockActual { get; set; }
    public decimal? PrecioCompra { get; set; }
    public decimal? PrecioVenta { get; set; }
}
