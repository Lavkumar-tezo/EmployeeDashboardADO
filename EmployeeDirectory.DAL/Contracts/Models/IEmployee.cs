namespace EmployeeDirectory.DAL.Contracts.Models
{
    public interface IEmployee
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        DateOnly JoinDate { get; set; }

        public string Location { get; set; }

        public string Department { get; set; }

        public string JobTitle { get; set; }
    }
}
