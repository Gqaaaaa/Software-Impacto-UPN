namespace Impacto.Inventory.Api.Models;

public class Usuario
{
    public string Id { get; set; } = string.Empty;
    public string? NombreUsuario { get; set; }
    public string? Contrasena { get; set; }
    public bool? Estado { get; set; }
    public string? IdEmpleado { get; set; }
    public Empleado? Empleado { get; set; }
    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
}
