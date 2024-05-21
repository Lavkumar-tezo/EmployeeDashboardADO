using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.DAL.Contracts.Providers;
namespace EmployeeDirectory.BAL.Providers
{
    public class GetProjectDeptList(IGenericRepository data):IGetProjectDeptList
    {
        private readonly IGenericRepository _deptProjectOperations = data;

        public Dictionary<string, string> GetList(string tableName)
        {
            return _deptProjectOperations.GetList(tableName);
        }
    }
}
