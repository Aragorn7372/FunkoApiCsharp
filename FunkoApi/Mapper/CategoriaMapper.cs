using FunkoApi.Dto.Categories;
using FunkoApi.Models;

namespace FunkoApi.mapper;

public static class CategoriaMapper
{
    public static Categoria ToModel(this CategoriaRequestDto categoria)
    {
        return new Categoria
        {
            Nombre = categoria.Nombre,
        };
    }

    public static CategoriaResponseDto ToDto(this Categoria categoria)
    {
        return new CategoriaResponseDto(categoria.Id, categoria.Nombre);
    }
}