namespace EmployeeManagement.Domain.Entities;

public class Employee : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SupervisorId { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Employee? Supervisor { get; set; }
    public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
}
