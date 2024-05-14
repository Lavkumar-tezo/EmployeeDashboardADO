using EmployeeDirectory.DAL.Models;
namespace EmployeeDirectory.BAL.Interfaces
{
    public interface IRoleProvider
    {
        public void AddRole(Dictionary<string, string> inputs);

        public string GenerateRoleId();

        public List<Role> GetRoles();
    }
}
