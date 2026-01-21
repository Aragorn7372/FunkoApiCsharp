using FunkoApi.Service;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace FunkoApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FilesController(
    IStorageService storageService
    )
    : ControllerBase
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();


    /// <summary>
    /// Descarga un archivo del servidor
    /// </summary>
    [HttpGet("download/{fileName}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(
        string fileName,
        [FromQuery] string? folder = "uploads")
    {
        try
        {
            var stream = await storageService.LoadAsStreamAsync(fileName, folder);
            var contentType = GetContentType(fileName);

            return File(stream, contentType, fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Archivo no encontrado",
                Detail = $"El archivo '{fileName}' no existe"
            });
        }
    }

    /// <summary>
    /// Obtiene la URL pública de un archivo
    /// </summary>
    [HttpGet("url/{fileName}")]
    [ProducesResponseType(typeof(FileUrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUrl(string fileName, [FromQuery] string? folder = "images")
    {
        if (!storageService.Exists(fileName, folder))
            return NotFound();

        return Ok(new FileUrlResponse
        {
            FileName = fileName,
            Url = storageService.GetUrl(fileName, folder)
        });
    }

    /// <summary>
    /// Serve archivos estáticos (para desarrollo)
    /// </summary>
    [HttpGet("serve/{**path}")]
    public IActionResult ServeFile(string path)
    {
        var filePath = Path.Combine(storageService.GetFilePath(path), path);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var contentType = GetContentType(path);
        var file = System.IO.File.OpenRead(filePath);

        return File(file, contentType);
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }

    public class FileUrlResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}