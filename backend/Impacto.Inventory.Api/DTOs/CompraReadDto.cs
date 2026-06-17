namespace Impacto.Inventory.Api.DTOs;

public class CompraReadDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime? Fecha { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumDocumento { get; set; }
    public decimal? Igv { get; set; }
    public string? Estado { get; set; }
    public string? IdProveedor { get; set; }
    public string? Proveedor { get; set; }
    public string? IdUsuario { get; set; }
    public string? Usuario { get; set; }
    public List<DetalleCompraReadDto> Detalles { get; set; } = new();
}
