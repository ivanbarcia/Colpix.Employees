using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ValidateCredentialsAsync(string username, string password);
}
