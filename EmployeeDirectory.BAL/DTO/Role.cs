using EmployeeDirectory.BAL.Interfaces;

namespace EmployeeDirectory.BAL.DTO
{
    public class Role : IRole
    {
        public required string Name { get; set; }

        public required string Department { get; set; }

        public required string Location { get; set; }

        public string? Description { get; set; }
    }
}
