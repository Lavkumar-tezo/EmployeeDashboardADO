using EmployeeDirectory.DAL.Contracts.Providers;
using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.Connections
{
    public class DbConnection(string connectionString) : IDbConnection
    {
        private readonly string _connectionString = connectionString;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
