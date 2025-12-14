using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);

        if (user == null)
        {
            return false;
        }

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
