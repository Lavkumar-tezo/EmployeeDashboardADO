namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IDeptProjectOperations
    {
        public Dictionary<string, string> GetDepartments(string name);


    }
}
