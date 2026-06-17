namespace Impacto.Inventory.Api.DTOs;

public class ClienteCreateDto
{
    public string Id { get; set; } = string.Empty;
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Dni { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
