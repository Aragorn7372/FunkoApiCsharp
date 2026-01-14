using FunkoApi.dto;
using FunkoApi.Models;

namespace FunkoApi.mapper;

public static class FunkosMapper
{
    public static Funko ToModel(this FunkoRequestDto dto, Categoria categoria )
    {
        return new Funko(

            dto.Nombre,
            categoria,
            dto.Price);
    }

    public static FunkoResponseDto ToDto(this Funko funko)
    {
        return new FunkoResponseDto(
            funko.Id,
            funko.Name, 
            funko.Price, 
            funko.Category!.Nombre
        );
    }
}