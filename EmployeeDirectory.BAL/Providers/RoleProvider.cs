using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.DAL.Models;
using EmployeeDirectory.DAL.Contracts.Providers;
namespace EmployeeDirectory.BAL.Providers
{
    public class RoleProvider(IRoleRepository data) : IRoleProvider
    {
        private readonly IRoleRepository _roleOperations = data;

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
            _roleOperations.AddRole(role);
        }

        public string GenerateRoleId()
        {
            List<Role> roles = _roleOperations.GetRoles();
            if (roles.Count == 0)
            {
                return "IN001";
            }
            string LastRoleId = roles[^1].Id ?? "";
            int lastRoleNumber = int.Parse(LastRoleId[2..]);
            lastRoleNumber++;
            string newId = "IN" + lastRoleNumber.ToString("D3");
            return newId;
        }

        public List<Role> GetRoles()
        {
            return _roleOperations.GetRoles();
        }

        public List<Role> GetRolesByDept(string deptId)
        {
            return _roleOperations.GetRolesByDept(deptId);
        }
    }
}
