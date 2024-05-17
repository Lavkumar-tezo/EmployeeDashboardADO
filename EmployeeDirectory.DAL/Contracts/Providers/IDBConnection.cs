using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IDBConnection
    {
        public SqlConnection GetConnection();
    }
}
