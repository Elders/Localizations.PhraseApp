using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
