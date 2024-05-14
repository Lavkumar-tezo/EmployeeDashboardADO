using EmployeeDirectory.DAL.Models;
namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IDataProvider
    {
        public List<Employee> GetEmployees();

        public Dictionary<string, string[]> GetProjectDepartment();

        public void AddEmployee(Employee newEmp);

        public void UpdateEmployee(Employee emp, int index);

        public Employee GetEmpById(string empId);

        public void DeleteEmployee(string id);

        public List<Role> GetRoles();

        public void AddRole(Role newRole);

    }
}
