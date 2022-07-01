using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Filters;
using Reusables.Repositories;
using Reusables.Repositories.EFCore;
using Reusables.Storage.Models;

namespace Reusables.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommon(this IServiceCollection services)
        {
            services.AddScoped<TaskCancelledExceptionFilterAttribute>();
        }

        public static void AddEfCore(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AdventureWorksContext>((dbContextConfig) =>
            {
                dbContextConfig.UseSqlServer(config.GetConnectionString("AdventureWorks"));
            });

            services.AddScoped<IProductsRepository, EFCoreProductsRepository>();
        }

        public static void AddEfCoreImproved(this IServiceCollection services, IConfiguration config)
        {
            const int PoolSize = 3000;

            services.AddDbContextPool<AdventureWorksContext>(
                dbContextConfig =>
                {
                    dbContextConfig.UseSqlServer(config.GetConnectionString("AdventureWorks"));
                    dbContextConfig.EnableThreadSafetyChecks(enableChecks: false);
                },
                poolSize: PoolSize);

            services.AddScoped<IProductsRepository, EFCoreImprovedProductsRepository>();
        }
    }
}
