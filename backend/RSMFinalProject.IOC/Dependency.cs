namespace RSMFinalProject.IOC
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RSMFinalProject.BLL.Services;
    using RSMFinalProject.BLL.Services.Contract;
    using RSMFinalProject.DAL.DbContext;
    using RSMFinalProject.DAL.Repositories;
    using RSMFinalProject.DAL.Repositories.Contract;
    using System.Reflection;

    public static class Dependency
    {
        private static readonly Assembly s_businessLogicLayerAssembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => assembly.FullName is not null && assembly.FullName.Contains("BLL"))
                .First();

        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddDbContext<AdventureWorksContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("localConnection"));
            });

            services.AddValidatorsFromAssembly(s_businessLogicLayerAssembly);

            services.AddAutoMapper(s_businessLogicLayerAssembly);

            services.AddScoped<ISalesOrderHeaderRepository, SalesOrderHeaderRepository>();

            services.AddScoped<ISalesOrderHeaderService, SalesOrderHeaderService>();
        }
    }
}
