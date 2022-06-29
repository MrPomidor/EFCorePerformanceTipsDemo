using Reusables.DI;

namespace EFCoreDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            CondigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            Configure(app);

            app.Run();
        }

        private static void CondigureServices(IServiceCollection services, ConfigurationManager config)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                    );

            services.AddEfCore(config);
        }

        private static void Configure(WebApplication app)
        {
            app.MapControllers();
        }
    }
}