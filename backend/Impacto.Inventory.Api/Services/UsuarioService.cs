using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IRolRepository _rolRepository;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IEmpleadoRepository empleadoRepository,
        IRolRepository rolRepository)
    {
        _usuarioRepository = usuarioRepository;
        _empleadoRepository = empleadoRepository;
        _rolRepository = rolRepository;
    }

    public async Task<List<UsuarioReadDto>> GetAllAsync()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        return usuarios.Select(ConvertirAReadDto).ToList();
    }

    public async Task<UsuarioReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var usuario = await _usuarioRepository.GetByIdAsync(id.Trim());
        return usuario is null ? null : ConvertirAReadDto(usuario);
    }

    public async Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto dto)
    {
        await ValidarCreateDto(dto);

        var usuario = new Usuario
        {
            Id = dto.Id.Trim(),
            NombreUsuario = LimpiarTexto(dto.Usuario),
            // Pendiente: aplicar hash de contrasena antes de guardar en produccion.
            Contrasena = LimpiarTexto(dto.Contrasena),
            Estado = dto.Estado,
            IdEmpleado = LimpiarTexto(dto.IdEmpleado)
        };

        var usuarioCreado = await _usuarioRepository.CreateAsync(usuario, dto.IdRol);
        var usuarioConRelaciones = await _usuarioRepository.GetByIdAsync(usuarioCreado.Id);
        return ConvertirAReadDto(usuarioConRelaciones ?? usuarioCreado);
    }

    public async Task<bool> UpdateAsync(string id, UsuarioUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();
        await ValidarUpdateDto(idLimpio, dto);

        if (!await _usuarioRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        var usuario = new Usuario
        {
            Id = idLimpio,
            NombreUsuario = LimpiarTexto(dto.Usuario),
            // Pendiente: aplicar hash de contrasena antes de guardar en produccion.
            Contrasena = LimpiarTexto(dto.Contrasena),
            Estado = dto.Estado,
            IdEmpleado = LimpiarTexto(dto.IdEmpleado)
        };

        return await _usuarioRepository.UpdateAsync(usuario, dto.IdRol);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var idLimpio = id.Trim();

        if (!await _usuarioRepository.ExistsAsync(idLimpio))
        {
            return false;
        }

        return await _usuarioRepository.DeleteAsync(idLimpio);
    }

    private async Task ValidarCreateDto(UsuarioCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id del usuario es obligatorio.");
        }

        if (await _usuarioRepository.ExistsAsync(dto.Id.Trim()))
        {
            throw new InvalidOperationException("Ya existe un usuario con el id indicado.");
        }

        await ValidarDatosUsuario(dto.Usuario, dto.Contrasena, dto.IdEmpleado, dto.IdRol);
    }

    private async Task ValidarUpdateDto(string id, UsuarioUpdateDto dto)
    {
        await ValidarDatosUsuario(dto.Usuario, dto.Contrasena, dto.IdEmpleado, dto.IdRol, id);
    }

    private async Task ValidarDatosUsuario(string? usuario, string? contrasena, string? idEmpleado, string? idRol, string? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(usuario))
        {
            throw new ArgumentException("El nombre de usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(contrasena))
        {
            throw new ArgumentException("La contrasena es obligatoria.");
        }

        ValidarLongitud(usuario, 50, "El nombre de usuario no puede superar 50 caracteres.");
        ValidarLongitud(contrasena, 100, "La contrasena no puede superar 100 caracteres.");
        ValidarLongitud(idEmpleado, 100, "El id del empleado no puede superar 100 caracteres.");
        ValidarLongitud(idRol, 100, "El id del rol no puede superar 100 caracteres.");

        if (await _usuarioRepository.NombreUsuarioExistsAsync(usuario.Trim(), excludeId))
        {
            throw new InvalidOperationException("Ya existe un usuario con el nombre indicado.");
        }

        if (!string.IsNullOrWhiteSpace(idEmpleado) && !await _empleadoRepository.ExistsAsync(idEmpleado.Trim()))
        {
            throw new ArgumentException("El empleado indicado no existe.");
        }

        if (!string.IsNullOrWhiteSpace(idRol) && !await _rolRepository.ExistsAsync(idRol.Trim()))
        {
            throw new ArgumentException("El rol indicado no existe.");
        }
    }

    private static UsuarioReadDto ConvertirAReadDto(Usuario usuario)
    {
        return new UsuarioReadDto
        {
            Id = usuario.Id,
            Usuario = usuario.NombreUsuario,
            Estado = usuario.Estado,
            IdEmpleado = usuario.IdEmpleado,
            Empleado = CrearNombreEmpleado(usuario.Empleado),
            Roles = usuario.UsuarioRoles
                .Select(usuarioRol => usuarioRol.Rol?.Nombre ?? usuarioRol.IdRol)
                .ToList()
        };
    }

    private static string? CrearNombreEmpleado(Empleado? empleado)
    {
        if (empleado is null)
        {
            return null;
        }

        return string.Join(" ", new[] { empleado.Nombres, empleado.Apellidos }
            .Where(valor => !string.IsNullOrWhiteSpace(valor)));
    }

    private static string? LimpiarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    private static void ValidarLongitud(string? valor, int maximo, string mensaje)
    {
        if (!string.IsNullOrWhiteSpace(valor) && valor.Trim().Length > maximo)
        {
            throw new ArgumentException(mensaje);
        }
    }
}
