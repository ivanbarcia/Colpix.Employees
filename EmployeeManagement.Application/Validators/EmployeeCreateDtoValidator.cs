using EmployeeManagement.Application.DTOs;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class EmployeeCreateDtoValidator : AbstractValidator<EmployeeCreateDto>
{
    public EmployeeCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(200).WithMessage("Email cannot exceed 200 characters");
    }
}
