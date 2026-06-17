namespace Impacto.Inventory.Api.DTOs;

public class UsuarioReadDto
{
    public string Id { get; set; } = string.Empty;
    public string? Usuario { get; set; }
    public bool? Estado { get; set; }
    public string? IdEmpleado { get; set; }
    public string? Empleado { get; set; }
    public List<string> Roles { get; set; } = new();
}
