namespace Impacto.Inventory.Api.Models;

public class Venta
{
    public string Id { get; set; } = string.Empty;
    public DateTime? Fecha { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumDocumento { get; set; }
    public decimal? Igv { get; set; }
    public string? Estado { get; set; }
    public string? IdCliente { get; set; }
    public string? IdUsuario { get; set; }
    public Cliente? Cliente { get; set; }
    public Usuario? Usuario { get; set; }
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}
