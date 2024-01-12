using Localizations.PhraseApp.Infrastructure;
using Localizations.PhraseApp.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Localizations.PhraseApp
{
    public static class PhraseAppServiceCollectionExtensions
    {
        public static IServiceCollection AddPhraseApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddOption<PhraseAppOptions, PhraseAppOptionsProvider>();
            services.AddSingleton<PhraseAppLocalizationCacheFactory>();
            services.AddSingleton<ILocalizationFactory, PhraseAppLocalizationFactory>();
            services.AddSingleton<PhraseAppLocalizationFactory>();
            services.AddSingleton<PhraseAppLocalizationCache>();// Hey-yo
            services.AddPhraseAppDefault(configuration);

            return services;
        }

        public static IServiceCollection AddPhraseAppWithMultitenancy(this IServiceCollection services, IConfiguration configuration, Func<IServiceProvider, string> tenantProvider)
        {
            services.AddOptions();
            services.AddOption<PhraseAppOptions, PhraseAppOptionsProvider>();
            services.AddSingleton<PhraseAppLocalizationCacheFactory>();
            services.AddSingleton<ILocalizationFactory, PhraseAppLocalizationFactory>();
            services.AddSingleton<PhraseAppLocalizationFactory>();
            services.AddSingleton<PhraseAppLocalizationCache>();// Hey-yo
            services.AddPhraseAppTenants(configuration);

            return services;
        }

        private static IServiceCollection AddPhraseAppDefault(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new PhraseAppOptions();
            configuration.GetSection(PhraseAppOptionsProvider.SectionDefault).Bind(options);
            string tenant = options.Tenant ?? PhraseAppOptionsProvider.NoTenant;

            services.AddHttpClient<PhraseAppLocalization, PhraseAppLocalization>($"{PhraseAppLocalizationFactory.HttpClientPrefix}_{tenant}", client =>
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
}
