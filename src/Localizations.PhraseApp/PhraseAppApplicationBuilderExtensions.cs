using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Localizations.PhraseApp
{
    public static class PhraseAppApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> UsePhraseApp(this IApplicationBuilder app)
        {
            PhraseAppLocalizationFactory localizationFactory = app.ApplicationServices.GetRequiredService<PhraseAppLocalizationFactory>();
            await localizationFactory.InitializeAndCacheAllAsync().ConfigureAwait(false);

            return app;
        }

        public static IHost UsePhraseApp(this IHost host)
        {
            var localization = host.Services.GetRequiredService<PhraseAppLocalization>();
            localization.CacheLocalesAndTranslationsAsync().GetAwaiter().GetResult();

            return host;
        }
    }
}
