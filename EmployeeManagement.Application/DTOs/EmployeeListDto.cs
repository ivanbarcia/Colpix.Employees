namespace EmployeeManagement.Application.DTOs;

public class EmployeeListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SupervisorId { get; set; }
    public DateTime UpdatedAt { get; set; }
}
