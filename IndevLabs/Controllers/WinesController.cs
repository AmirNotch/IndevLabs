using IndevLabs.Models.db;
using IndevLabs.Models.Wines;
using IndevLabs.Service;
using IndevLabs.Validation;
using IndevLabs.Validation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndevLabs.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/public")]
public class WinesController : BaseController
{
    private readonly WineService _wineService;

    public WinesController(IValidationStorage validationStorage, WineService wineService) 
        : base(validationStorage)
    {
        _wineService = wineService;
    }
    
    [HttpGet("wines")]
    public async Task<IActionResult> GetWines(CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _wineService.GetWines(token), ct);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("wineById")]
    public async Task<IActionResult> GetWineById([FromQuery, ValidInt] int wineId, CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _wineService.GetWineById(wineId, token), ct);
    }
    
    [Authorize]
    [HttpPost("createWine")]
    public async Task<IActionResult> CreateWine([FromBody] WineDto wineDto, CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _wineService.CreateWine(wineDto, token), ct);
    }
    
    [Authorize]
    [HttpPut("updateWine")]
    public async Task<IActionResult> UpdateWine([FromBody] Wine wine, CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _wineService.UpdateWine(wine, token), ct);
    }
    
    [Authorize]
    [HttpDelete("deleteWine")]
    public async Task<IActionResult> DeleteWine([FromQuery, ValidInt] int wineId, CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _wineService.DeleteWine(wineId, token), ct);
    }
}