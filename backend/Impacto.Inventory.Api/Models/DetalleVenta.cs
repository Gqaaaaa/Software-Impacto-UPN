namespace Impacto.Inventory.Api.Models;

public class DetalleVenta
{
    public string Id { get; set; } = string.Empty;
    public string? IdVenta { get; set; }
    public string? IdProducto { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
    public Venta? Venta { get; set; }
    public Producto? Producto { get; set; }
}
