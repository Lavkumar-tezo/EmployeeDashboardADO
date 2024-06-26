﻿using System.Globalization;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.Exceptions;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.BAL.Helper;
using EmployeeDirectory.BAL.Extension;
using Employee = EmployeeDirectory.DAL.Models.Employee;
using EmployeeDirectory.DAL.Models;

namespace EmployeeDirectory.BAL.Providers
{
    public class EmployeeProvider(IGetProperty prop, IEmployeeRepository employeeOperations,IGetProjectDeptList list,IRoleProvider role) : IEmpProvider
    {
        private readonly IGetProperty _getProperty = prop;
        private static int _employeeIndex;
        private static string _employeeId = "";
        private readonly IEmployeeRepository _employeeOperations =employeeOperations;
        private readonly IGetProjectDeptList _employeeDeptList=list;
        private readonly IRoleProvider _role = role;

        public DTO.Employee AddValueToEmp(Dictionary<string, string> values, string mode)
        {
            if (string.Equals("Add",mode))
            {
                DTO.Employee newEmp = new()
                {
                    FirstName = values["FirstName"],
                    LastName = values["LastName"],
                    Email = values["Email"],
                    JoinDate = DateTime.Parse(values["JoinDate"]),
                    Location = values["Location"],
                    JobTitle = values["JobTitle"],
                    Manager = values["Manager"],
                    Mobile = values["Mobile"],
                    Project = values["Project"],
                    Department = values["Department"],
                    DOB = (values["DOB"] != null) ? DateTime.Parse(values["DOB"]) : DateTime.Now
                };
                return newEmp;
            }
            else
            {
                (bool check,Employee emp) = GetEmployeeById(values["Id"]);
                AssignIdToModel(emp);
                DTO.Employee updateEmp = new()
                {
                    FirstName = (values["FirstName"].IsEmpty()) ? emp.FirstName : values["FirstName"],
                    LastName = (values["LastName"].IsEmpty()) ? emp.LastName : values["LastName"],
                    Location = (values["Location"].IsEmpty()) ? emp.Location : values["Location"],
                    Email = (values["Email"].IsEmpty()) ? emp.Email : values["Email"],
                    JobTitle = (values["JobTitle"].IsEmpty()) ? emp.JobTitle : values["JobTitle"],
                    Manager = (values["Manager"].IsEmpty()) ? emp.Manager : values["Manager"],
                    Project = (values["Project"].IsEmpty()) ? emp.Project : values["Project"],
                    JoinDate = (values["JoinDate"].IsEmpty()) ? emp.JoinDate : DateTime.Parse(values["JoinDate"]),
                    Department = (values["Department"].IsEmpty()) ? emp.Department : values["Department"],
                    Mobile = (values["Mobile"]).IsEmpty()? emp.Mobile : values["Mobile"],
                    DOB=(values["DOB"].IsEmpty())? emp.DOB:DateTime.Parse(values["DOB"])
                };
                if (!values["DOB"].IsEmpty())
                {
                    updateEmp.DOB = DateTime.Parse(values["DOB"]);
                }
                else if(!emp.DOB.HasValue)
                {
                    updateEmp.DOB = DateTime.Now;
                }
                else
                {
                    updateEmp.DOB = emp.DOB;
                }
                return updateEmp;
            }
        }

        public void AssignIdToModel(Employee emp)
        {
            Dictionary<string, string> deptList = _employeeDeptList.GetList("Department");
            Dictionary<string, string> projectList = _employeeDeptList.GetList("Project");
            if (emp.Project !=null)
            {
                emp.Project = projectList.FirstOrDefault(x=> x.Value.Equals(emp.Project)).Key;
            }
            emp.Department = deptList.FirstOrDefault(x => x.Value.Equals(emp.Department)).Key;
            List<Role> roles = _role.GetRolesByDept(emp.Department);
            foreach (Role role in roles)
            {
                if (emp.JobTitle.Equals(role.Name))
                {
                    emp.JobTitle = role.Id;
                }
            }

        }

        public DateTime ConvertIntoDate(string input)
        {
            DateTime date;
            if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out date))
            {
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public Employee AssignValueToModel(DTO.Employee emp)
        {
            if (_employeeId.IsEmpty())
            {
                _employeeId = GenerateEmpId();
            }
            Employee newEmp = new()
            {
                Id = _employeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Location = emp.Location,
                Project = emp.Project,
                Department=emp.Department,
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
            _employeeOperations.AddEmployee(AssignValueToModel(employee));
        }

        public List<Employee> GetEmployees()
        {
            return _employeeOperations.GetEmployees();
        }

        public (bool, Employee) GetEmployeeById(string id)
        {
            id = id.ToUpper();
            List<Employee> employeeList = GetEmployees();
            _employeeIndex = employeeList.FindIndex(emp => emp.Id == id);
            if (_employeeIndex != -1)
            {
                _employeeId = employeeList[_employeeIndex].Id;
                ModelKeyStore.deptId= employeeList[_employeeIndex].Department;
                return (true, employeeList[_employeeIndex]);
            }
            throw new EmpNotFound("Employee Not found with given Id");

        }

        public bool IsEmployeePresent(string id)
        {
            (bool check, Employee employee) = GetEmployeeById(id);
            MessagesInputStore.inputFieldValues = _getProperty.GetValueFromObject(employee);
            return check;
        }

        public void UpdateEmployee(DTO.Employee employee)
        {
            _employeeOperations.UpdateEmployee(AssignValueToModel(employee));

        }

        public void DeleteEmployee(string id)
        {
            _employeeOperations.DeleteEmployee(id);

        }

        public string GenerateEmpId()
        {
            List<Employee> employees = _employeeOperations.GetEmployees();
            if (employees.Count == 0)
            {
                return "TZ0001";
            }
            string LastEmpId = employees.Last().Id;
            int lastEmpNumber = int.Parse(LastEmpId[2..]);
            lastEmpNumber++;
            string newId = "TZ" + lastEmpNumber.ToString("D4");
            return newId;
        }

    }
}
