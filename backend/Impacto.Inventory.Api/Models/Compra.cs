namespace Impacto.Inventory.Api.Models;

public class Compra
{
    public string Id { get; set; } = string.Empty;
    public DateTime? Fecha { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumDocumento { get; set; }
    public decimal? Igv { get; set; }
    public string? Estado { get; set; }
    public string? IdProveedor { get; set; }
    public string? IdUsuario { get; set; }
    public Proveedor? Proveedor { get; set; }
    public Usuario? Usuario { get; set; }
    public ICollection<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();
}
