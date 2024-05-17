using EmployeeDirectory.DAL.Contracts.Providers;
using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.DataOperations
{
    public class DeptProjectOperations(IDBConnection dBConnection):IDeptProjectOperations
    {
        private readonly IDBConnection _dBConnection = dBConnection;

        public Dictionary<string, string> GetDepartments(string tableName)
        {
            Dictionary<string, string> list = [];
            try
            {
                using (SqlConnection connection=_dBConnection.GetConnection())
                {
                    string query = $"SELECT * FROM {tableName}";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(reader["ID"].ToString() ?? "", reader["Name"].ToString() ?? "");
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
