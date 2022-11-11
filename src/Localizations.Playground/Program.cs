using Localizations.PhraseApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Localizations.Playground
{
    class Program
    {
        static async Task Main()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddLogging();
            services.AddPhraseApp(cfg);
            services.AddSingleton<IConfiguration>(cfg);

            services.AddTransient<PhraseAppLocalizationFactory>();

            var serviceProvider = services.BuildServiceProvider();

            var opts = serviceProvider.GetRequiredService<IOptions<PhraseAppOptions>>();
            string testKey = "1vipcustomer";

            // multi-tenancy
            var fact = serviceProvider.GetRequiredService<PhraseAppLocalizationFactory>();
            var noTenantLocatlization = await fact.GetLocalizationAsync("notenant");
            var test1 = await noTenantLocatlization.GetAsync("branch", "en");

            var pruvitTenantLocatlization = await fact.GetLocalizationAsync("tenant2");
            var test2 = await pruvitTenantLocatlization.GetAsync(testKey, "bg");
        }
    }
}
