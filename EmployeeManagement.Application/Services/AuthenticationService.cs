using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticationService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var isValid = await _userRepository.ValidateCredentialsAsync(request.Username, request.Password);

        if (!isValid)
        {
            throw new UnauthorizedException("Invalid username or password");
        }

        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
        {
            throw new UnauthorizedException("Invalid username or password");
        }

        var token = _jwtTokenService.GenerateToken(user);
        var expiresAt = _jwtTokenService.GetTokenExpiration();

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt
        };
    }
}
