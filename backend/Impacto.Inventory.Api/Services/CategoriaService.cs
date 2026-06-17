using Impacto.Inventory.Api.DTOs;
using Impacto.Inventory.Api.Models;
using Impacto.Inventory.Api.Repositories;

namespace Impacto.Inventory.Api.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<CategoriaReadDto>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return categorias.Select(ConvertirAReadDto).ToList();
    }

    public async Task<CategoriaReadDto?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var categoria = await _categoriaRepository.GetByIdAsync(id.Trim());
        return categoria is null ? null : ConvertirAReadDto(categoria);
    }

    public async Task<CategoriaReadDto> CreateAsync(CategoriaCreateDto dto)
    {
        ValidarCategoriaCreateDto(dto);

        var existeCategoria = await _categoriaRepository.ExistsAsync(dto.Id.Trim());

        if (existeCategoria)
        {
            throw new InvalidOperationException("Ya existe una categoria con el id indicado.");
        }

        var categoria = new Categoria
        {
            Id = dto.Id.Trim(),
            Descripcion = dto.Descripcion.Trim()
        };

        var categoriaCreada = await _categoriaRepository.CreateAsync(categoria);
        return ConvertirAReadDto(categoriaCreada);
    }

    public async Task<bool> UpdateAsync(string id, CategoriaUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        ValidarCategoriaUpdateDto(dto);

        var existeCategoria = await _categoriaRepository.ExistsAsync(id.Trim());

        if (!existeCategoria)
        {
            return false;
        }

        var categoria = new Categoria
        {
            Id = id.Trim(),
            Descripcion = dto.Descripcion.Trim()
        };

        return await _categoriaRepository.UpdateAsync(categoria);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var existeCategoria = await _categoriaRepository.ExistsAsync(id.Trim());

        if (!existeCategoria)
        {
            return false;
        }

        return await _categoriaRepository.DeleteAsync(id.Trim());
    }

    private static CategoriaReadDto ConvertirAReadDto(Categoria categoria)
    {
        return new CategoriaReadDto
        {
            Id = categoria.Id,
            Descripcion = categoria.Descripcion
        };
    }

    private static void ValidarCategoriaCreateDto(CategoriaCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            throw new ArgumentException("El id de la categoria es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            throw new ArgumentException("La descripcion de la categoria es obligatoria.");
        }
    }

    private static void ValidarCategoriaUpdateDto(CategoriaUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            throw new ArgumentException("La descripcion de la categoria es obligatoria.");
        }
    }
}
