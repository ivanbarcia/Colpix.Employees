using EmployeeManagement.Application.DTOs;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
{
    public EmployeeUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(200).WithMessage("Email cannot exceed 200 characters");

        RuleFor(x => x.SupervisorId)
            .Must((dto, supervisorId) => supervisorId != dto.Id)
            .When(x => x.SupervisorId.HasValue)
            .WithMessage("An employee cannot be their own supervisor");
    }
}
