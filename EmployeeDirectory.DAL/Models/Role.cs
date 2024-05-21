using EmployeeDirectory.DAL.Contracts.Models;

namespace EmployeeDirectory.DAL.Models
{
    public class Role : IRole
    {
        public required string Name { get; set; }

        public required string Department { get; set; }

        public required string Location { get; set; }

        public required string Id { get; set; }

        public string? Description { get; set; }
    }
}

