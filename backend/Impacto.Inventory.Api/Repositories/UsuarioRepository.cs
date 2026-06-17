using Impacto.Inventory.Api.Data;
using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ImpactoDbContext _context;

    public UsuarioRepository(ImpactoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .AsNoTracking()
            .Include(usuario => usuario.Empleado)
            .Include(usuario => usuario.UsuarioRoles)
                .ThenInclude(usuarioRol => usuarioRol.Rol)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(string id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .Include(usuario => usuario.Empleado)
            .Include(usuario => usuario.UsuarioRoles)
                .ThenInclude(usuarioRol => usuarioRol.Rol)
            .FirstOrDefaultAsync(usuario => usuario.Id == id);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario, string? idRol = null)
    {
        _context.Usuarios.Add(usuario);

        if (!string.IsNullOrWhiteSpace(idRol))
        {
            _context.UsuarioRoles.Add(new UsuarioRol
            {
                IdUsuario = usuario.Id,
                IdRol = idRol.Trim(),
                Estado = true
            });
        }

        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> UpdateAsync(Usuario usuario, string? idRol = null)
    {
        var usuarioExistente = await _context.Usuarios
            .Include(item => item.UsuarioRoles)
            .FirstOrDefaultAsync(item => item.Id == usuario.Id);

        if (usuarioExistente is null)
        {
            return false;
        }

        usuarioExistente.NombreUsuario = usuario.NombreUsuario;
        usuarioExistente.Contrasena = usuario.Contrasena;
        usuarioExistente.Estado = usuario.Estado;
        usuarioExistente.IdEmpleado = usuario.IdEmpleado;

        if (!string.IsNullOrWhiteSpace(idRol))
        {
            usuarioExistente.UsuarioRoles.Clear();
            usuarioExistente.UsuarioRoles.Add(new UsuarioRol
            {
                IdUsuario = usuario.Id,
                IdRol = idRol.Trim(),
                Estado = true
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var usuario = await _context.Usuarios
            .Include(item => item.UsuarioRoles)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (usuario is null)
        {
            return false;
        }

        _context.UsuarioRoles.RemoveRange(usuario.UsuarioRoles);
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Usuarios
            .AnyAsync(usuario => usuario.Id == id);
    }

    public async Task<bool> NombreUsuarioExistsAsync(string usuario, string? excludeId = null)
    {
        return await _context.Usuarios
            .AnyAsync(item => item.NombreUsuario == usuario && (excludeId == null || item.Id != excludeId));
    }
}
