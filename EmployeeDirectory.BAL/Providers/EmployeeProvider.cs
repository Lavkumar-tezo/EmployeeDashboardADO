using System.Globalization;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.Exceptions;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.BAL.Helper;
using EmployeeDirectory.BAL.Extension;
using Employee = EmployeeDirectory.DAL.Models.Employee;

namespace EmployeeDirectory.BAL.Providers
{
    public class EmployeeProvider(IDataProvider data, IGetProperty prop) : IEmpProvider
    {
        private readonly IDataProvider _dataOperations = data;
        private readonly IGetProperty _getProperty = prop;
        private static int employeeIndex;
        private static string employeeId = "";

        public DTO.Employee AddValueToEmp(Dictionary<string, string> values, string mode)
        {
            if (mode == "Add")
            {
                DateOnly joinDate = ConvertIntoDate(values["JoinDate"]);
                DTO.Employee newEmp = new()
                {
                    FirstName = values["FirstName"],
                    LastName = values["LastName"],
                    Email = values["Email"],
                    JoinDate = joinDate,
                    Location = values["Location"],
                    JobTitle = values["JobTitle"],
                    Department = values["Department"],
                    Manager = values["Manager"],
                    Mobile = values["Mobile"],
                    Project = values["Project"]
                };
                DateOnly dob = ConvertIntoDate(values["DOB"]);
                if (dob != DateOnly.MinValue)
                {
                    newEmp.DOB = dob;
                }
                return newEmp;
            }
            else
            {
                (bool check, dynamic emp) = GetEmployeeById(values["Id"]);
                if (check)
                {
                    DateOnly dob = ConvertIntoDate(values["JoinDate"]);
                    DTO.Employee updateEmp = new()
                    {
                        FirstName = (values["FirstName"].IsEmpty()) ? emp.FirstName : values["FirstName"],
                        LastName = (values["LastName"].IsEmpty()) ? emp.LastName : values["LastName"],
                        Location = (values["Location"].IsEmpty()) ? emp.Location : values["Location"],
                        Email = (values["Email"].IsEmpty()) ? emp.Email : values["Email"],
                        JobTitle = (values["JobTitle"].IsEmpty()) ? emp.JobTitle : values["JobTitle"],
                        Department = (values["Department"].IsEmpty()) ? emp.Department : values["Department"],
                        Manager = (values["Manager"].IsEmpty()) ? emp.Manager : values["Manager"],
                        Project = (values["Project"].IsEmpty()) ? emp.Project : values["Project"],
                        JoinDate = dob,
                    };
                    dob = ConvertIntoDate(values["DOB"]);
                    if (dob != DateOnly.MinValue)
                    {
                        updateEmp.DOB = dob;
                    }
                    return updateEmp;
                }
                else
                {
                    return emp;
                }
            }
        }

        public DateOnly ConvertIntoDate(string input)
        {
            if (DateOnly.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out DateOnly result))
            {
                return result;
            }
            return DateOnly.MinValue;
        }

        public Employee AssignValueToModel(DTO.Employee emp)
        {
            if (employeeId.IsEmpty())
            {
                employeeId = GenerateEmpId();
            }
            Employee newEmp = new()
            {
                Id = employeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Location = emp.Location,
                Department = emp.Department,
                Project = emp.Project,
                Manager = emp.Manager,
                JoinDate = emp.JoinDate,
                DOB = emp.DOB,
                Email = emp.Email,
                JobTitle = emp.JobTitle,
                Mobile = emp.Mobile,
            };
            return newEmp;
        }

        public void AddEmployee(DTO.Employee employee)
        {
            try
            {
                _dataOperations.AddEmployee(AssignValueToModel(employee));
            }
            catch (IOException)
            {
                throw;
            }
        }

        public List<Employee> GetEmployees()
        {
            try
            {
                return _dataOperations.GetEmployees();
            }
            catch (IOException)
            {
                throw;
            }

        }

        public (bool, Employee) GetEmployeeById(string id)
        {
            id = id.ToUpper();
            try
            {
                List<Employee> employeeList = GetEmployees();
                employeeIndex = employeeList.FindIndex(emp => emp.Id == id);
                if (employeeIndex != -1)
                {
                    employeeId = employeeList[employeeIndex].Id;
                    return (true, employeeList[employeeIndex]);
                }
                throw new EmpNotFound("Employee Not found with given Id");
            }
            catch (IOException)
            {
                throw;
            }
            catch (EmpNotFound)
            {
                throw;
            }

        }

        public bool IsEmployeePresent(string id)
        {
            try
            {
                (bool check, dynamic employee) = GetEmployeeById(id);
                if (check)
                {
                    MessagesInputStore.inputFieldValues = _getProperty.GetValueFromObject(employee);
                }
                return check;
            }
            catch (EmpNotFound)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }

        }

        public void UpdateEmployee(DTO.Employee employee)
        {
            try
            {
                _dataOperations.UpdateEmployee(AssignValueToModel(employee), employeeIndex);
                employeeId = "";
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
                _dataOperations.DeleteEmployee(id);
            }
            catch (IOException)
            {
                throw;
            }

        }

        public List<DAL.Models.Role> GetRoles()
        {
            try
            {
                return _dataOperations.GetRoles();
            }
            catch (IOException)
            {
                throw;
            }

        }

        public string GenerateEmpId()
        {
            try
            {
                List<Employee> employees = _dataOperations.GetEmployees();
                string LastEmpId = employees.Last().Id;
                int lastEmpNumber = int.Parse(LastEmpId[2..]);
                lastEmpNumber++;
                string newId = "TZ" + lastEmpNumber.ToString("D4");
                return newId;
            }
            catch (IOException)
            {
                throw;
            }
        }

    }
}
