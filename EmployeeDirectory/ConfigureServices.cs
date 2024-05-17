using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.BAL.Providers;
using EmployeeDirectory.BAL.Validators;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.DataOperations;
using EmployeeDirectory.BAL.Interfaces.Views;
using EmployeeDirectory.DAL.Connections;
namespace EmployeeDirectory
{
    public class ConfigureServices
    {
        public static IServiceProvider BuildServices()
        {
            string connectionString = @"Server=SQL-DEV;Database=LavDB_ADO;Trusted_Connection=True;TrustServerCertificate=True";
            var services = new ServiceCollection();

            services.AddScoped<IEmployeeView, Views.Employee>();
            services.AddScoped<IRoleView, Views.Role>();
            services.AddScoped<IValidator, Validator>();
            services.AddScoped<IRole, BAL.DTO.Role>();
            services.AddScoped<IEmpProvider, EmployeeProvider>();
            services.AddScoped<IGetProjectDeptList, GetProjectDeptList>();
            services.AddScoped<IGetProperty, GetProperty>();
            services.AddScoped<IRoleProvider, RoleProvider>();
            services.AddScoped<IRoleOperations, RoleOperations>();
            services.AddScoped<IEmployeeOperations, EmployeeOperations>();
            services.AddSingleton<IDBConnection>(new DBConnection(connectionString));
            //services.AddScoped<IDBConnection>(provider => new DBConnection(connectionString));
            services.AddScoped<IDeptProjectOperations, DeptProjectOperations>();
            //services.AddScoped<IDataProvider, DataOperations>();
           // services.AddScoped<DataOperations>();
            services.AddScoped<MainMenu>();

            return services.BuildServiceProvider();
        }

    }
}
