using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.Exceptions;
using EmployeeDirectory.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeDirectory.DAL.DataOperations
{
    public class EmployeeOperations(IDBConnection dBConnection):IEmployeeOperations
    {
        private readonly IDBConnection _dBConnection = dBConnection;

        public List<Employee> GetEmployees()
        {

            List<Employee> list = [];
            try
            {
                using (SqlConnection connection=_dBConnection.GetConnection())
                {
                    string query = "SELECT emp.ID as [ID],emp.FirstName,emp.LastName,emp.Email,emp.Location,emp.JoiningDate, p.Name as [Project],emp.Manager,emp.DOB,emp.mobile,role.Name as [JobTitle],dept.Name as [Department] FROM Employee emp JOIN Role role On emp.JobTitle=role.ID JOIN Department dept On role.DepartmentID=dept.ID LEFT JOIN Project p ON p.ID=emp.ProjectID";
                    using (SqlCommand cmd = new(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Employee emp = new Employee
                            {
                                Id = reader["ID"].ToString() ?? "",
                                FirstName = reader["FirstName"].ToString() ?? "",
                                LastName = reader["LastName"].ToString() ?? "",
                                Email = reader["Email"].ToString() ?? "",
                                JobTitle = reader["JobTitle"].ToString() ?? "",
                                Department = reader["Department"].ToString()??"",
                                JoinDate = (DateTime)reader["JoiningDate"],
                                Location = reader["Location"].ToString() ?? "",
                                Manager = reader["Manager"].ToString() ?? "",
                                Project = reader["Project"].ToString() ?? "",
                                Mobile = reader["Mobile"].ToString() ?? ""                               
                            };
                            if(reader["DOB"] != DBNull.Value)
                            {
                                emp.DOB = (DateTime)reader["DOB"];
                            }
                            list.Add(emp);
                        }

                    }
                    connection.Close();

                }
                return list;
            }
            catch (Exception)
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
            catch(Exception)
            {
                throw;
            }
        }

        public void AddEmployee(Employee newEmp)
        {
            try
            {
                using(SqlConnection connection= _dBConnection.GetConnection())
                {
                    string query = "INSERT INTO Employee (ID, FirstName,LastName,Email,JoiningDate,Location,JobTitle,ProjectID,Manager,DepartmentID,Mobile,DOB) Values (@ID, @FirstName,@LastName,@Email,@JoiningDate,@Location,@JobTitle,@Project,@Manager,@DepartmentID,@Mobile,@DOB)";
                    using (SqlCommand cmd = new(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", newEmp.Id);
                        cmd.Parameters.AddWithValue("@FirstName", newEmp.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", newEmp.LastName);
                        cmd.Parameters.AddWithValue("@Email", newEmp.Email);
                        cmd.Parameters.AddWithValue("@JoiningDate", newEmp.JoinDate);
                        cmd.Parameters.AddWithValue("@Location", newEmp.Location);
                        cmd.Parameters.AddWithValue("@JobTitle", newEmp.JobTitle);
                        cmd.Parameters.AddWithValue("@DepartmentID",newEmp.Department);
                        cmd.Parameters.AddWithValue("@Project", newEmp.Project.IsNullOrEmpty() ? DBNull.Value : newEmp.Project);
                        cmd.Parameters.AddWithValue("@Manager", newEmp.Manager.IsNullOrEmpty() ? DBNull.Value : newEmp.Manager);
                        cmd.Parameters.AddWithValue("@Mobile", newEmp.Mobile.IsNullOrEmpty() ? DBNull.Value : newEmp.Mobile);
                        cmd.Parameters.AddWithValue("@DOB", (newEmp.DOB==DateTime.Now) ? DBNull.Value : newEmp.DOB);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateEmployee(Employee emp)
        {
            try
            {
                using (SqlConnection connection = _dBConnection.GetConnection())
                {
                    string query = "UPDATE Employee SET  FirstName=@FirstName,LastName=@LastName,[Email]=@EmailAd,JoiningDate=@JoiningDate,DepartmentID=@DepartmentID,Location=@Location,JobTitle=@JobTitle,ProjectID=@Project,Manager=@Manager,Mobile=@Mobile,DOB=@DOB";
                    using (SqlCommand cmd = new (query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", emp.Id);
                        cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                        cmd.Parameters.AddWithValue("@EmailAd", emp.Email);
                        cmd.Parameters.AddWithValue("@JoiningDate", emp.JoinDate);
                        cmd.Parameters.AddWithValue("@Location", emp.Location);
                        cmd.Parameters.AddWithValue("@JobTitle", emp.JobTitle);
                        cmd.Parameters.AddWithValue("@DepartmentID", emp.Department);
                        cmd.Parameters.AddWithValue("@Project", emp.Project.IsNullOrEmpty() ? DBNull.Value : emp.Project);
                        cmd.Parameters.AddWithValue("@Manager", emp.Manager.IsNullOrEmpty() ? DBNull.Value : emp.Manager);
                        cmd.Parameters.AddWithValue("@Mobile", emp.Mobile.IsNullOrEmpty() ? DBNull.Value : emp.Mobile);
                        cmd.Parameters.AddWithValue("@DOB", (emp.DOB == DateTime.Now) ? DBNull.Value : emp.DOB);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteEmployee(string id)
        {
            try
            {
                using (SqlConnection connection = _dBConnection.GetConnection())
                {
                    string query = @"DELETE FROM Employee WHERE ID=@ID";
                    using (SqlCommand cmd = new(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
