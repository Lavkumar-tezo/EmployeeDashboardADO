using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IDbConnection
    {
        public SqlConnection GetConnection();
    }
}
