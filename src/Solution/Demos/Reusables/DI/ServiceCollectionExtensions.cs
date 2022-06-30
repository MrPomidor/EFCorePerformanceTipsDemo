using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;
using Reusables.Repositories.EFCore;
using Reusables.Storage.Models;

namespace Reusables.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEfCore(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AdventureWorksContext>((dbContextConfig) =>
            {
                dbContextConfig.UseSqlServer(config.GetConnectionString("AdventureWorks"));
            });

            services.AddScoped<IProductsRepository, EFCoreProductsRepository>();
        }
    }
}
