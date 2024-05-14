using Newtonsoft.Json;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.Exceptions;
using EmployeeDirectory.DAL.Models;
namespace EmployeeDirectory.DAL.DataOperations
{
    public class DataOperations : IDataProvider
    {
        public List<Employee> GetEmployees()
        {

            try
            {
                string data = File.ReadAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\Employee.json");
                List<Employee> Employees = JsonConvert.DeserializeObject<List<Employee>>(data) ?? [];
                return Employees;
            }
            catch (IOException)
            {
                throw;
            }
        }

        public Dictionary<string, string[]> GetProjectDepartment()
        {
            try
            {
                string staticData = File.ReadAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\StaticData.json");
                Dictionary<string, string[]>? data = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(staticData);
                if (data != null)
                {
                    return data;
                }
                return [];
            }
            catch (System.Text.Json.JsonException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
        }

        public Employee GetEmpById(string empId)
        {
            try
            {
                List<Employee>? employees = GetEmployees();
                Employee? employee = employees.FirstOrDefault(e => e.Id == empId);
                if (employee != null)
                {
                    return employee;
                }
                throw new EmpNotFound("Id not found");
            }
            catch (EmpNotFound)
            {
                throw;
            }
        }

        public void AddEmployee(Employee newEmp)
        {
            try
            {
                List<Employee>? Employees = GetEmployees();
                Employees?.Add(newEmp);
                string jsonString = JsonConvert.SerializeObject(Employees);
                File.WriteAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\Employee.json", jsonString);
            }
            catch (IOException)
            {
                throw;
            }
        }

        public void UpdateEmployee(Employee emp, int index)
        {
            try
            {
                List<Employee> EmployeeList = GetEmployees();
                EmployeeList[index] = emp;
                string jsonString = JsonConvert.SerializeObject(EmployeeList);
                File.WriteAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\Employee.json", jsonString);
            }
            catch (IOException)
            {
                throw;
            }
        }

        public void DeleteEmployee(string id)
        {
            try
            {
                List<Employee>? EmployeeList = GetEmployees();
                int index = EmployeeList.FindIndex(employee => employee.Id.Equals(id));
                if (index != -1)
                {
                    EmployeeList.RemoveAt(index);
                    string jsonString = JsonConvert.SerializeObject(EmployeeList);
                    File.WriteAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\Employee.json", jsonString);
                }
            }
            catch (IOException)
            {
                throw;
            }

        }

        public List<Role> GetRoles()
        {
            try
            {
                string data = File.ReadAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\Role.json");
                List<Role> Roles = JsonConvert.DeserializeObject<List<Role>>(data) ?? [];
                return Roles;
            }
            catch (IOException)
            {
                throw;
            }
        }

        public void AddRole(Role newRole)
        {
            try
            {
                List<Role> Roles = GetRoles();
                Roles.Add(newRole);
                string jsonString = JsonConvert.SerializeObject(Roles);
                File.WriteAllText(@"C:\Users\lav.k\OneDrive - Technovert\Desktop\tezo\C#-Task\EmployeeDirectory.DAL\Data\Role.json", jsonString);
            }
            catch (IOException)
            {
                throw;
            }
        }

    }
}
