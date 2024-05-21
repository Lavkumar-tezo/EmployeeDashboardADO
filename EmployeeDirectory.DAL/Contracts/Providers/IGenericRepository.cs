namespace EmployeeDirectory.DAL.Contracts.Providers
{
    public interface IGenericRepository
    {
        public Dictionary<string, string> GetList(string name);
    }
}
