namespace Impacto.Inventory.Api.Models;

public class UsuarioRol
{
    public string IdUsuario { get; set; } = string.Empty;
    public string IdRol { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public Usuario? Usuario { get; set; }
    public Rol? Rol { get; set; }
}
