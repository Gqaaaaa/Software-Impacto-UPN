namespace Impacto.Inventory.Api.DTOs;

public class UsuarioCreateDto
{
    public string Id { get; set; } = string.Empty;
    public string? Usuario { get; set; }
    public string? Contrasena { get; set; }
    public bool? Estado { get; set; }
    public string? IdEmpleado { get; set; }
    public string? IdRol { get; set; }
}
