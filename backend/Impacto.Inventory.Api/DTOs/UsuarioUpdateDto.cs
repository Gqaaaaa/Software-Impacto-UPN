namespace Impacto.Inventory.Api.DTOs;

public class UsuarioUpdateDto
{
    public string? Usuario { get; set; }
    public string? Contrasena { get; set; }
    public bool? Estado { get; set; }
    public string? IdEmpleado { get; set; }
    public string? IdRol { get; set; }
}
