namespace Impacto.Inventory.Api.Models;

public class Proveedor
{
    public string Id { get; set; } = string.Empty;
    public string? RazonSocial { get; set; }
    public string? Ruc { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
