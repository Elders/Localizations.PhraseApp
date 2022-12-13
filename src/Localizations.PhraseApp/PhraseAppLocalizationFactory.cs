using Localizations.PhraseApp.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Localizations.PhraseApp
{
    public class PhraseAppLocalizationFactory
    {
        internal const string HttpClientPrefix = "phraseapp";

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly PhraseAppLocalizationCacheFactory cacheFactory;
        private readonly ILogger<PhraseAppLocalizationFactory> logger;

        public PhraseAppLocalizationFactory(IHttpClientFactory httpClientFactory, IConfiguration configuration, PhraseAppLocalizationCacheFactory cacheFactory, ILogger<PhraseAppLocalizationFactory> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.cacheFactory = cacheFactory;
            this.logger = logger;
        }

        public Task<ILocalization> GetLocalizationAsync(string tenant)
        {
            var options = GetTenantOptions(tenant);

            return InitializeLocalizationAsync(options);
        }

        public async Task<ILocalization> InitializeLocalizationAsync(PhraseAppOptions options)
        {
            string httpClientName = $"{PhraseAppLocalizationFactory.HttpClientPrefix}_{options.Tenant}";

            PhraseAppLocalizationCache cache = cacheFactory.GetCache(options.Tenant);
            var localization = new PhraseAppLocalization(httpClientFactory.CreateClient(httpClientName), Options.Create(options), cache, logger);
            await localization.CacheLocalesAndTranslationsAsync();

            return localization;
        }

        public async Task InitializeAndCacheAllAsync()
        {
            List<PhraseAppOptions> tenantOptions = new List<PhraseAppOptions>();
            configuration.GetSection(PhraseAppOptionsProvider.SectionTenantConfig)?.Bind(tenantOptions);

            foreach (var item in tenantOptions)
            {
                await InitializeLocalizationAsync(item).ConfigureAwait(false);
            }
        }

        private PhraseAppOptions GetTenantOptions(string tenant)
        {
            List<PhraseAppOptions> tenantOptions = new List<PhraseAppOptions>();
            configuration.GetSection(PhraseAppOptionsProvider.SectionTenantConfig)?.Bind(tenantOptions);

            PhraseAppOptions foundOptions = tenantOptions.Where(opt => opt.Tenant.Equals(tenant)).SingleOrDefault();
            if (foundOptions is null)
            {
                foundOptions = new PhraseAppOptions();
                configuration.GetSection(PhraseAppOptionsProvider.SectionDefault).Bind(foundOptions);
                foundOptions.Tenant ??= PhraseAppOptionsProvider.NoTenant;
            }

            return foundOptions;
        }
    }
}
