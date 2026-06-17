namespace Impacto.Inventory.Api.DTOs;

public class DetalleVentaReadDto
{
    public string Id { get; set; } = string.Empty;
    public string? IdProducto { get; set; }
    public string? Producto { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
}
