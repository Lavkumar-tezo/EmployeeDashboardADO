using EmployeeDirectory.DAL.Contracts.Models;

namespace EmployeeDirectory.DAL.Models
{
    public class Employee : IEmployee
    {
        public required string Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required DateOnly JoinDate { get; set; }

        public required string Location { get; set; }

        public required string Department { get; set; }

        public required string JobTitle { get; set; }

        public string? Project { get; set; }

        public string? Mobile { get; set; }

        public DateOnly? DOB { get; set; }

        public string? Manager { get; set; }
    }
}
