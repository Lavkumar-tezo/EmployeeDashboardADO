using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.DAL.Models;
using EmployeeDirectory.DAL.Contracts.Providers;
namespace EmployeeDirectory.BAL.Providers
{
    public class RoleProvider(IDataProvider data) : IRoleProvider, IComparer<Role>
    {
        private readonly IDataProvider _dataOperations = data;

        public void AddRole(Dictionary<string, string> inputs)
        {
            Role role = new()
            {
                Name = inputs["Name"],
                Location = inputs["Location"],
                Department = inputs["Department"],
                Description = inputs["Description"],
                Id = GenerateRoleId()
            };
            _dataOperations.AddRole(role);
        }

        public string GenerateRoleId()
        {
            try
            {
                List<Role> roles = _dataOperations.GetRoles();
                roles.Sort(Compare);
                string LastRoleId = roles[^1].Id ?? "";
                int lastRoleNumber = int.Parse(LastRoleId[2..]);
                lastRoleNumber++;
                string newId = "IN" + lastRoleNumber.ToString("D3");
                return newId;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
        }

        public int Compare(Role? x, Role? y)
        {
            if (x != null && y != null)
            {
                return x.Id.CompareTo(y.Id);
            }
            return 0;
        }

        public List<Role> GetRoles()
        {
            try
            {
                return _dataOperations.GetRoles();
            }
            catch (IOException)
            {
                throw;
            }
        }
    }
}
