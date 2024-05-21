using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.BAL.Providers;
using EmployeeDirectory.BAL.Validators;
using EmployeeDirectory.DAL.Contracts.Providers;
using EmployeeDirectory.DAL.Repositories;
using EmployeeDirectory.BAL.Interfaces.Views;
using EmployeeDirectory.DAL.Connections;
using Microsoft.Extensions.Configuration;
namespace EmployeeDirectory
{
    public class ConfigureServices
    {
        public static IServiceProvider BuildServices()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("app-settings.json").Build();
            string connectionString = configBuilder["connection:sql"]!;
            var services = new ServiceCollection();

            services.AddScoped<IEmployeeView, Views.Employee>();
            services.AddScoped<IRoleView, Views.Role>();
            services.AddScoped<IValidator, Validator>();
            services.AddScoped<IRole, BAL.DTO.Role>();
            services.AddScoped<IEmpProvider, EmployeeProvider>();
            services.AddScoped<IGetProjectDeptList, GetProjectDeptList>();
            services.AddScoped<IGetProperty, GetProperty>();
            services.AddScoped<IRoleProvider, RoleProvider>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDbConnection>(provider => new DbConnection(connectionString));
            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<MainMenu>();

            return services.BuildServiceProvider();
        }

    }
}
