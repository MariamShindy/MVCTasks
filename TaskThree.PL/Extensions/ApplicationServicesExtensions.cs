using Microsoft.Extensions.DependencyInjection;
using TaskThree.BLL;
using TaskThree.BLL.Interfaces;
using TaskThree.BLL.Repositories;

namespace TaskThree.PL.Extensions
{
	public static class ApplicationServicesExtensions
	{
       public static IServiceCollection AppApplicationServices(this IServiceCollection services)
		{
			//services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			//services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
			//services.AddTransient<IDepartmentRepository, DepartmentRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
