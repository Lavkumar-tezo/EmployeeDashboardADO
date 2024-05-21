using EmployeeDirectory.DAL.Models;

namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IRoleRepository
    {
        public List<Role> GetRoles();

        public void AddRole(Role newRole);

        public List<Role> GetRolesByDept(string deptId);
    }
}
