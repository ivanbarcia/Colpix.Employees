namespace EmployeeManagement.Application.DTOs;

public class EmployeeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SupervisorId { get; set; }
}
