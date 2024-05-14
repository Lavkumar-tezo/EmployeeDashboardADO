using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.BAL.Interfaces
{
    public interface IGetProjectDeptList
    {
        public string[] GetStaticData(string input);
    }
}
