using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.BAL.Providers;
using EmployeeDirectory.BAL.Validators;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.DataOperations;
using EmployeeDirectory.BAL.Interfaces.Views;
namespace EmployeeDirectory
{
    public class ConfigureServices
    {
        public static IServiceProvider BuildServices()
        {
            var services = new ServiceCollection();

            services.AddScoped<IEmployeeView, Views.Employee >();
            services.AddScoped<IRoleView, Views.Role>();
            services.AddScoped<IValidator, Validator>();
            services.AddScoped<IEmployee, BAL.DTO.Employee>();
            services.AddScoped<IRole, BAL.DTO.Role>();
            services.AddScoped<IEmpProvider, EmployeeProvider>();
            services.AddScoped<IGetProjectDeptList, GetProjectDeptList>();
            services.AddScoped<IGetProperty, GetProperty>();
            services.AddScoped<IRoleProvider, RoleProvider>();
            services.AddScoped<DAL.Contracts.Models.IEmployee, DAL.Models.Employee>();
            services.AddScoped<DAL.Contracts.Models.IRole, DAL.Models.Role>();
            services.AddScoped<IDataProvider, DataOperations>();
            services.AddScoped<MainMenu>();

            return services.BuildServiceProvider();
        }

    }
}
