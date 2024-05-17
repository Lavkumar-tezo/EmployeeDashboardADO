namespace EmployeeDirectory.BAL.Interfaces
{
    public interface IGetProjectDeptList
    {
        public Dictionary<string, string> GetList(string name);
    }
}
