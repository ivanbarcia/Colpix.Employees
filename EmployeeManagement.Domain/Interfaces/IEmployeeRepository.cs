using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<int> GetSubordinatesCountAsync(int employeeId);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> ExistsAsync(int id);
}
