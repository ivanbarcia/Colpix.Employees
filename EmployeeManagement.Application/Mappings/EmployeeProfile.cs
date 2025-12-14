using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeListDto>();

        CreateMap<Employee, EmployeeDetailDto>()
            .ForMember(dest => dest.SubordinatesCount, opt => opt.Ignore());

        CreateMap<EmployeeCreateDto, Employee>();

        CreateMap<EmployeeUpdateDto, Employee>();
    }
}
