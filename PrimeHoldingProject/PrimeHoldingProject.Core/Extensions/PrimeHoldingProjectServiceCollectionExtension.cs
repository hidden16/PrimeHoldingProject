using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Services;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories;
using PrimeHoldingProject.Infrastructure.Data.Common.Repositories.Contracts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PrimeHoldingProjectServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}
