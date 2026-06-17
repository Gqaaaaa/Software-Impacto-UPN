namespace Impacto.Inventory.Api.DTOs;

public class ProductoUpdateDto
{
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? PrecioCompra { get; set; }
    public decimal? PrecioVenta { get; set; }
    public int? Stock { get; set; }
    public string? IdCategoria { get; set; }
}
