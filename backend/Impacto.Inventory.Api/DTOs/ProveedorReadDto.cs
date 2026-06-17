namespace Impacto.Inventory.Api.DTOs;

public class ProveedorReadDto
{
    public string Id { get; set; } = string.Empty;
    public string? RazonSocial { get; set; }
    public string? Ruc { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
