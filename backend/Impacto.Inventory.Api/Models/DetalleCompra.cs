namespace Impacto.Inventory.Api.Models;

public class DetalleCompra
{
    public string Id { get; set; } = string.Empty;
    public string? IdCompra { get; set; }
    public string? IdProducto { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
    public Compra? Compra { get; set; }
    public Producto? Producto { get; set; }
}
