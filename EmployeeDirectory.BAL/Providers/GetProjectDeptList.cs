using EmployeeDirectory.BAL.Interfaces;
using System.Text.Json;
using EmployeeDirectory.DAL.Contracts.Providers;
namespace EmployeeDirectory.BAL.Providers
{
    public class GetProjectDeptList(IDataProvider data) : IGetProjectDeptList
    {
        private readonly IDataProvider _dataOperations = data;

        /// <summary>
        /// Gets the static data for department or project.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>List of Department or Project</returns>
        public string[] GetStaticData(string input)
        {
            try
            {
                Dictionary<string, string[]> list = _dataOperations.GetProjectDepartment();
                return list[input];
            }
            catch (IOException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }

        }
    }
}
