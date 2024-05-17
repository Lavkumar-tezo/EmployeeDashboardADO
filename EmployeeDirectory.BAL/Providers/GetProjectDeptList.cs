using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.Models;
namespace EmployeeDirectory.BAL.Providers
{
    public class GetProjectDeptList(IDeptProjectOperations data):IGetProjectDeptList
    {
        private readonly IDeptProjectOperations _deptProjectOperations = data;

        public Dictionary<string, string> GetList(string tableName)
        {
            try
            {
                return _deptProjectOperations.GetDepartments(tableName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
