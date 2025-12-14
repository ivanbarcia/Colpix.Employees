using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IValidator<EmployeeCreateDto> _createValidator;
    private readonly IValidator<EmployeeUpdateDto> _updateValidator;

    public EmployeesController(
        IEmployeeService employeeService,
        IValidator<EmployeeCreateDto> createValidator,
        IValidator<EmployeeUpdateDto> updateValidator)
    {
        _employeeService = employeeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<EmployeeListDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailDto>> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailDto>> Create([FromBody] EmployeeCreateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
            });
        }

        var employee = await _employeeService.CreateEmployeeAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmployeeDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailDto>> Update(int id, [FromBody] EmployeeUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { message = "ID in URL does not match ID in request body" });
        }

        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
            });
        }

        var employee = await _employeeService.UpdateEmployeeAsync(dto);
        return Ok(employee);
    }
}
