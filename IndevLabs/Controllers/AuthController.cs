using IndevLabs.Models.Auth;
using IndevLabs.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IndevLabs.Controllers;

[ApiController]
[Route("api/public")]
public class AuthController : ControllerBase
{
    private readonly AuthorizationService _authorizationService;

    public AuthController(AuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var token = _authorizationService.Login(request.Username, request.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}