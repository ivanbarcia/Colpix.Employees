namespace EmployeeManagement.Domain.Entities;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
