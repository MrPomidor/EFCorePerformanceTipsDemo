using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reusables;
using Reusables.DI;
using Reusables.Storage.Models;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class UtilsTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private ServiceProvider EfCoreDefaultServiceProvider;

        public UtilsTests(ITestOutputHelper output)
        {
            _output = output;
            EfCoreDefaultServiceProvider = BuildEFServiceProvider();
        }

        private ServiceProvider BuildEFServiceProvider()
        {
            var config = GetConfiguration();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEfCore(config);

            return serviceCollection.BuildServiceProvider();
        }

        private IConfigurationRoot GetConfiguration() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        [Fact(Skip = "Cleanup for POST scenarious. Should run manually")]
        public async Task CleanupTestProducts()
        {
            const int itemsToTake = 5_000;
            const string testColor = Consts.ApplicationProductsColor;

            using var efServiceProviderScope = EfCoreDefaultServiceProvider.CreateScope();
            var dbContext = efServiceProviderScope.ServiceProvider.GetRequiredService<AdventureWorksContext>();

            _output.WriteLine("Start removing test products ...");

            long removedItems = 0;
            while (true)
            {
                var testColors = await dbContext.Products.Where(x => x.Color == testColor).Take(itemsToTake).ToListAsync();
                if (testColors.Count == 0)
                    break;

                dbContext.Products.RemoveRange(testColors);
                await dbContext.SaveChangesAsync();

                removedItems += testColors.Count;

                _output.WriteLine("Batch removed ...");
            }

            _output.WriteLine($"Test products successfully cleaned. Cleaned items count: {removedItems}");
        }

        public void Dispose()
        {
            try
            {
                EfCoreDefaultServiceProvider?.Dispose();
                EfCoreDefaultServiceProvider = null;
            }
            catch { }
        }
    }
}
