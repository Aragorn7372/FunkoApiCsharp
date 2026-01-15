using CSharpFunctionalExtensions;
using FunkoApi.dto;
using FunkoApi.Error;
using FunkoApi.Service;

using Microsoft.AspNetCore.Mvc;



namespace FunkoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FunkosController(IService service):ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FunkoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await service.GetFunkosAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FunkoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(long id)
    {
        
        return await service.GetFunkoAsync(id).Match(
            onSuccess: response => Ok(response),
            onFailure: error=> error switch
            {
                FunkoNotFoundError=> NotFound(new { message = error.Error }),
                _=> StatusCode(500,new  { message = error.Error })
            });
    }

    [HttpPost]
    [ProducesResponseType(typeof(FunkoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] FunkoRequestDto request)
    {
        return await service.SaveFunkoAsync(request).Match(
            onSuccess: response => CreatedAtAction(nameof(GetAsync), new { id = response.Id }, response),
            onFailure: error => error switch
            {
                FunkoValidationError => BadRequest(new { message = error.Error }),
                _ => StatusCode(500, new { message = error.Error })
            });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FunkoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAsync(long id, [FromBody] FunkoRequestDto request)
    {
        return await service.UpdateFunkoAsync(id, request).Match(
            onSuccess: response => Ok(response),
            onFailure: error => error switch
            {
                FunkoValidationError => BadRequest(new { message = error.Error }),
                FunkoNotFoundError => NotFound(new { message = error.Error }),
                _ => StatusCode(500, new { message = error.Error })
            });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(FunkoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        return await service.DeleteFunkoAsync(id).Match(
            onSuccess: response => Ok(response),
            onFailure: error => error switch
            {
                FunkoNotFoundError => NotFound(new { message = error.Error }),
                _ => StatusCode(500, new { message = error.Error })
            });
    }
    
    
}