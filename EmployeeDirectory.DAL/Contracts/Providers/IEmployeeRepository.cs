using EmployeeDirectory.DAL.Models;


namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetEmployees();

        public Employee GetEmpById(string empId);

        public void AddEmployee(Employee newEmp);

        public void UpdateEmployee(Employee emp);

        public void DeleteEmployee(string id);
    }
}
