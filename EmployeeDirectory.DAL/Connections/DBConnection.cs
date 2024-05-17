using EmployeeDirectory.DAL.Contracts.Providers;
using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.Connections
{
    public class DBConnection:IDBConnection
    {
        private readonly string _connectionString;

        public DBConnection(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
