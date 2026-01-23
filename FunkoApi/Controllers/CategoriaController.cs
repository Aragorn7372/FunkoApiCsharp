using CSharpFunctionalExtensions;
using FunkoApi.Dto.Categories;
using FunkoApi.Error;
using FunkoApi.Service.Category;
using Microsoft.AspNetCore.Mvc;

namespace FunkoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriaController(ICategoriaService service) :ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await service.GetCategoriasAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync(string id)
    {
        return await service.GetCategoriaAsync(id).Match(
            onSuccess: response => Ok(response),
            onFailure: error=> error switch
            {
                CategoriaNotFoundError=> NotFound(new { message = error.Error }),
                _=> StatusCode(500,new  { message = error.Error })
            });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] CategoriaRequestDto categoria)
    {
        return await service.UpdateCategoriaAsync(id,categoria).Match(
            onSuccess: response => Ok(response),
            onFailure: error=> error switch
            {
                CategoriaNotFoundError=> NotFound(new { message = error.Error }),
                _=> StatusCode(500,new  { message = error.Error })
            });
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] CategoriaRequestDto categoria)
    {
        return await service.SaveCategoriaAsync(categoria).Match(
            onSuccess: response => Created($"/api/categoria/{response.Id}", response), 
            onFailure: error => error switch
            {
                CategoriaBadRequestError => BadRequest(new { message = error.Error }),
                _ => StatusCode(500, new { message = error.Error })
            });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(CategoriaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await service.DeleteCategoriaAsync(id).Match(
            onSuccess: response => Ok(response),
            onFailure: error=> error switch
            {
                CategoriaNotFoundError=> NotFound(new { message = error.Error }),
                _=> StatusCode(500,new  { message = error.Error })
            });
    }
    
}