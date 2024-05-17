﻿using EmployeeDirectory.DAL.Connections;
using EmployeeDirectory.DAL.Models;
using EmployeeDirectory.DAL.Contracts.Providers;
using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.DataOperations
{
    public class RoleOperations(IDBConnection dBConnection) :IRoleOperations
    {
        private readonly IDBConnection _dBConnection = dBConnection;

        public List<Role> GetRoles()
        {
            List<Role> roles = [];

            try
            {
                using (SqlConnection connection = _dBConnection.GetConnection())
                {
                    string query = "SELECT role.Name,role.Location,role.Description,role.ID,dept.Name as [Department] FROM Role role JOIN Department dept ON role.DepartmentID=dept.ID;";
                    using(SqlCommand cmd = new(query,connection))
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Role role = new Role
                            {
                                Id = reader["ID"].ToString() ?? "",
                                Name = reader["Name"].ToString() ?? "",
                                Location = reader["Location"].ToString() ?? "",
                                Department = reader["Department"].ToString() ?? "",
                                Description = reader["Description"].ToString() ?? ""
                            };
                            roles.Add(role);
                        }
                    }
                }
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddRole(Role newRole)
        {
            try
            {
                using (SqlConnection connection = _dBConnection.GetConnection())
                {
                    string query = @"INSERT INTO Role (ID,Name,Location,DepartmentID,Description) VALUES (@ID,@Name,@Location,@DepartmentID,@Description)";
                    using (SqlCommand cmd = new(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID",newRole.Id);
                        cmd.Parameters.AddWithValue("@Name", newRole.Name);
                        cmd.Parameters.AddWithValue("@Location", newRole.Location);
                        cmd.Parameters.AddWithValue("@DepartmentID", newRole.Department);
                        cmd.Parameters.AddWithValue("@Description", newRole.Description);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Role> GetRolesByDept(string deptId)
        {
            List<Role> list = [];

            try
            {
                using (SqlConnection connection = _dBConnection.GetConnection())
                {
                    string query = "sELECT role.Name,role.Location,role.Description,role.ID,dept.Name as [Department] FROM Role role JOIN Department dept ON role.DepartmentID=dept.ID WHERE DepartmentID=@DeptId;";
                    using (SqlCommand cmd = new(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@DeptId", deptId);
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Role role = new Role
                            {
                                Id = reader["ID"].ToString() ?? "",
                                Name = reader["Name"].ToString() ?? "",
                                Location = reader["Location"].ToString() ?? "",
                                Department = reader["Department"].ToString() ?? "",
                                Description = reader["Description"].ToString() ?? ""
                            };
                            list.Add(role);
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
