using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
