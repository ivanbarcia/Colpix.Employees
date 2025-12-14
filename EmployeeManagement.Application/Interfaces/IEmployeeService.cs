using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeListDto>> GetAllEmployeesAsync();
    Task<EmployeeDetailDto> GetEmployeeByIdAsync(int id);
    Task<EmployeeDetailDto> CreateEmployeeAsync(EmployeeCreateDto dto);
    Task<EmployeeDetailDto> UpdateEmployeeAsync(EmployeeUpdateDto dto);
}
