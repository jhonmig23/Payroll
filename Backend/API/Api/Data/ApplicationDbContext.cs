using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure EmployeeNumber is unique
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeNumber)
                .IsUnique();

            // Seed some initial data
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    EmployeeNumber = "JOH-12345-15JAN1990",
                    LastName = "John",
                    FirstName = "Doe",
                    MiddleName = "Middle",
                    DateOfBirth = new DateTime(1990, 1, 15),
                    DailyRate = 100.00m,
                    WorkingDays = "MWF"
                },
                new Employee
                {
                    Id = 2,
                    EmployeeNumber = "SMI-67890-22FEB1985",
                    LastName = "Smith",
                    FirstName = "Jane",
                    MiddleName = "Ann",
                    DateOfBirth = new DateTime(1985, 2, 22),
                    DailyRate = 150.00m,
                    WorkingDays = "TTHS"
                }
            );
        }
    }
}