using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<LoginRequestDto> _validator;

    public AuthController(
        IAuthenticationService authenticationService,
        IValidator<LoginRequestDto> validator)
    {
        _authenticationService = authenticationService;
        _validator = validator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
            });
        }

        var response = await _authenticationService.LoginAsync(request);
        return Ok(response);
    }
}