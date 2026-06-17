namespace Impacto.Inventory.Api.DTOs;

public class VentaCreateDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime? Fecha { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumDocumento { get; set; }
    public decimal? Igv { get; set; }
    public string? Estado { get; set; }
    public string? IdCliente { get; set; }
    public string? IdUsuario { get; set; }
    public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
}
