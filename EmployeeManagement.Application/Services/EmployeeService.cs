using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeListDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EmployeeListDto>>(employees);
    }

    public async Task<EmployeeDetailDto> GetEmployeeByIdAsync(int id)
    {
        var employeeTask = _employeeRepository.GetByIdAsync(id);
        var countTask = _employeeRepository.GetSubordinatesCountAsync(id);

        await Task.WhenAll(employeeTask, countTask);

        var employee = await employeeTask;
        var subordinatesCount = await countTask;

        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), id);
        }

        var employeeDetail = _mapper.Map<EmployeeDetailDto>(employee);
        employeeDetail.SubordinatesCount = subordinatesCount;

        return employeeDetail;
    }

    public async Task<EmployeeDetailDto> CreateEmployeeAsync(EmployeeCreateDto dto)
    {
        if (dto.SupervisorId.HasValue)
        {
            var supervisorExists = await _employeeRepository.ExistsAsync(dto.SupervisorId.Value);
            if (!supervisorExists)
            {
                throw new NotFoundException(nameof(Employee), dto.SupervisorId.Value);
            }
        }

        var employee = _mapper.Map<Employee>(dto);
        var createdEmployee = await _employeeRepository.AddAsync(employee);

        var result = _mapper.Map<EmployeeDetailDto>(createdEmployee);
        result.SubordinatesCount = 0;

        return result;
    }

    public async Task<EmployeeDetailDto> UpdateEmployeeAsync(EmployeeUpdateDto dto)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(dto.Id);
        if (existingEmployee == null)
        {
            throw new NotFoundException(nameof(Employee), dto.Id);
        }

        if (dto.SupervisorId.HasValue)
        {
            if (dto.SupervisorId.Value == dto.Id)
            {
                throw new ValidationException("An employee cannot be their own supervisor");
            }

            var supervisorExists = await _employeeRepository.ExistsAsync(dto.SupervisorId.Value);
            if (!supervisorExists)
            {
                throw new NotFoundException(nameof(Employee), dto.SupervisorId.Value);
            }
        }

        var employee = _mapper.Map<Employee>(dto);
        var updatedEmployee = await _employeeRepository.UpdateAsync(employee);

        var subordinatesCount = await _employeeRepository.GetSubordinatesCountAsync(updatedEmployee.Id);

        var result = _mapper.Map<EmployeeDetailDto>(updatedEmployee);
        result.SubordinatesCount = subordinatesCount;

        return result;
    }
}
