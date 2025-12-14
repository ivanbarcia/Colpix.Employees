using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<int> GetSubordinatesCountAsync(int employeeId)
    {
        // Recursive function to count all direct and indirect subordinates
        var allEmployees = await _context.Employees.AsNoTracking().ToListAsync();

        var subordinatesCount = CountSubordinatesRecursive(employeeId, allEmployees);

        return subordinatesCount;
    }

    private int CountSubordinatesRecursive(int supervisorId, List<Employee> allEmployees)
    {
        // Get direct reports
        var directReports = allEmployees.Where(e => e.SupervisorId == supervisorId).ToList();

        var count = directReports.Count;

        // Recursively count subordinates of each direct report
        foreach (var report in directReports)
        {
            count += CountSubordinatesRecursive(report.Id, allEmployees);
        }

        return count;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        employee.CreatedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var existing = await _context.Employees.FindAsync(employee.Id);

        if (existing == null)
        {
            throw new KeyNotFoundException($"Employee with ID {employee.Id} not found");
        }

        existing.Name = employee.Name;
        existing.Email = employee.Email;
        existing.SupervisorId = employee.SupervisorId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
}
