using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBank.API.Repositories;
using MyBank.API.Services;
using WebApi.Services;

namespace MyBank.API.Infrastructure.Extensions
{
    public static class ConfigurationServiceExtensions
    {
        public static void SetupInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(new DatabaseOptions(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IBankingService, BankingService>();


        }
    }
}
