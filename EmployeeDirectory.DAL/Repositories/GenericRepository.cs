using EmployeeDirectory.DAL.Contracts.Providers;
using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.Repositories
{
    public class GenericRepository(IDbConnection dBConnection):IGenericRepository
    {
        private readonly IDbConnection _dBConnection = dBConnection;

        public Dictionary<string, string> GetList(string tableName)
        {
            Dictionary<string, string> list = [];
            using (SqlConnection connection = _dBConnection.GetConnection())
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
    }
}
