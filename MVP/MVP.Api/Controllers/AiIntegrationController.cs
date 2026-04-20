using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP.Services.IServices;

namespace MVP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AiIntegrationController(IOpenAIService openAIService) : ControllerBase
{
    private readonly IOpenAIService _openAIService = openAIService;


    [HttpPost("")]
    public async Task<IActionResult> AnalyzeServiceRequest([FromBody] ServiceRequestDto request)
    {
        var result = await _openAIService.GenerateServiceDescriptionAsync(request.Title, request.Description);
        return Ok(result);
    }


}

public class ServiceRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}