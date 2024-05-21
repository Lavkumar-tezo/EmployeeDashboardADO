namespace EmployeeDirectory.DAL.Contracts.Models
{
    public interface IRole
    {
        public string Name { get; set; }

        public string Department { get; set; }

        public string Location { get; set; }

        public string Id { get; set; }
    }
}