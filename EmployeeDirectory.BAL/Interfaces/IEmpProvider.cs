using Employee = EmployeeDirectory.DAL.Models.Employee;
namespace EmployeeDirectory.BAL.Interfaces
{
    public interface IEmpProvider
    {
        public void AddEmployee(DTO.Employee employee);

        public List<Employee> GetEmployees();

        public (bool, Employee) GetEmployeeById(string id);

        public bool IsEmployeePresent(string id);

        public DateOnly ConvertIntoDate(string input);

        public Employee AssignValueToModel(DTO.Employee emp);

        public List<DAL.Models.Role> GetRoles();

        public string GenerateEmpId();
        public void UpdateEmployee(DTO.Employee employee);

        public void DeleteEmployee(string id);

        public DTO.Employee AddValueToEmp(Dictionary<string, string> values, string mode);
    }
}
