using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Employee entity
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);

            // Self-referencing relationship
            entity.HasOne(e => e.Supervisor)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Email);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.HasIndex(u => u.Username)
                .IsUnique();
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed admin user (password: admin123)
        var adminUserId = 1;
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminUserId,
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            CreatedAt = DateTime.UtcNow
        });

        // Seed sample employees to demonstrate hierarchy
        var ceoId = 2;
        var ctoId = 3;
        var devManagerId = 4;
        var dev1Id = 5;
        var dev2Id = 6;
        var hrManagerId = 7;
        var hrSpecialistId = 8;

        var now = DateTime.UtcNow;

        modelBuilder.Entity<Employee>().HasData(
            // CEO - no supervisor
            new Employee
            {
                Id = ceoId,
                Name = "John Smith",
                Email = "john.smith@company.com",
                SupervisorId = null,
                CreatedAt = now,
                UpdatedAt = now
            },
            // CTO - reports to CEO
            new Employee
            {
                Id = ctoId,
                Name = "Sarah Johnson",
                Email = "sarah.johnson@company.com",
                SupervisorId = ceoId,
                CreatedAt = now,
                UpdatedAt = now
            },
            // Dev Manager - reports to CTO
            new Employee
            {
                Id = devManagerId,
                Name = "Michael Brown",
                Email = "michael.brown@company.com",
                SupervisorId = ctoId,
                CreatedAt = now,
                UpdatedAt = now
            },
            // Developer 1 - reports to Dev Manager
            new Employee
            {
                Id = dev1Id,
                Name = "Emily Davis",
                Email = "emily.davis@company.com",
                SupervisorId = devManagerId,
                CreatedAt = now,
                UpdatedAt = now
            },
            // Developer 2 - reports to Dev Manager
            new Employee
            {
                Id = dev2Id,
                Name = "David Wilson",
                Email = "david.wilson@company.com",
                SupervisorId = devManagerId,
                CreatedAt = now,
                UpdatedAt = now
            },
            // HR Manager - reports to CEO
            new Employee
            {
                Id = hrManagerId,
                Name = "Lisa Anderson",
                Email = "lisa.anderson@company.com",
                SupervisorId = ceoId,
                CreatedAt = now,
                UpdatedAt = now
            },
            // HR Specialist - reports to HR Manager
            new Employee
            {
                Id = hrSpecialistId,
                Name = "Robert Taylor",
                Email = "robert.taylor@company.com",
                SupervisorId = hrManagerId,
                CreatedAt = now,
                UpdatedAt = now
            }
        );
    }
}
