using Localizations.PhraseApp.Infrastructure;
using Localizations.PhraseApp.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Localizations.PhraseApp
{
    public static class PhraseAppServiceCollectionExtensions
    {
        public static IServiceCollection AddPhraseApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddOption<PhraseAppOptions, PhraseAppOptionsProvider>();
            services.AddSingleton<PhraseAppLocalizationCacheFactory>();// Hey-yo

            services.AddPhraseAppDefault(configuration);
            services.AddPhraseAppTenants(configuration);

            return services;
        }

        private static IServiceCollection AddPhraseAppDefault(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new PhraseAppOptions();
            configuration.GetSection(PhraseAppOptionsProvider.SectionDefault).Bind(options);

            services.AddHttpClient<PhraseAppLocalization, PhraseAppLocalization>($"{PhraseAppLocalizationFactory.HttpClientPrefix}_{PhraseAppOptionsProvider.NoTenant}", client =>
            {
                client.BaseAddress = new Uri(options.Address);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"token {options.AccessToken}");
            });

            return services;
        }

        private static IServiceCollection AddPhraseAppTenants(this IServiceCollection services, IConfiguration configuration)
        {
            List<PhraseAppOptions> tenantOptions = new List<PhraseAppOptions>();
            configuration.GetSection(PhraseAppOptionsProvider.SectionTenantConfig)?.Bind(tenantOptions);

            foreach (PhraseAppOptions tenantConfig in tenantOptions)
            {
                services.AddHttpClient<PhraseAppLocalization, PhraseAppLocalization>($"{PhraseAppLocalizationFactory.HttpClientPrefix}_{tenantConfig.Tenant}", client =>
                {
                    client.BaseAddress = new Uri(tenantConfig.Address);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"token {tenantConfig.AccessToken}");
                });
            }

            return services;
        }
    }

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

        public async Task<ILocalization> GetLocalizationAsync(string tenant)
        {
            string httpClientName = $"{PhraseAppLocalizationFactory.HttpClientPrefix}_{tenant}";
            PhraseAppOptions options = GetTenantOptions(tenant);
            PhraseAppLocalizationCache cache = cacheFactory.GetCache(tenant);
            var localization = new PhraseAppLocalization(httpClientFactory.CreateClient(httpClientName), Options.Create(options), cache, logger);
            await localization.CacheLocalesAndTranslationsAsync();

            return localization;
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
            }

            return foundOptions;
        }
    }
}
