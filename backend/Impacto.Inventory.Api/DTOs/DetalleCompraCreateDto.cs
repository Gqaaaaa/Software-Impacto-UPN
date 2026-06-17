namespace Impacto.Inventory.Api.DTOs;

public class DetalleCompraCreateDto
{
    public string Id { get; set; } = string.Empty;
    public string? IdProducto { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
}
