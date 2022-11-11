using Localizations.PhraseApp.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Localizations.PhraseApp.Internal
{
    internal class PhraseAppOptionsProvider : OptionsProviderBase<PhraseAppOptions>
    {
        internal const string NoTenant = "notenant";
        private const string DefaultAddress = "https://api.phraseapp.com/api/v2/";
        private const string DefaultLocale = "en";
        private const string DoNotUseStrictLocale = "false";
        private const string DefaultTtl = "5";

        public const string SectionDefault = "localization:phraseapp";
        public const string SectionTenantConfig = "localization:phraseapp:tenantConfig";

        public PhraseAppOptionsProvider(IConfiguration configuration) : base(configuration) { }

        public override void Configure(PhraseAppOptions options)
        {
            options.Tenant = configuration[$"{SectionDefault}:tenant"] ?? NoTenant;
            options.Address = configuration[$"{SectionDefault}:address"] ?? DefaultAddress;
            options.AccessToken = configuration[$"{SectionDefault}:accesstoken"];
            options.ProjectId = configuration[$"{SectionDefault}:projectid"];
            options.DefaultLocale = configuration[$"{SectionDefault}:defaultlocale"] ?? DefaultLocale;
            options.UseStrictLocale = bool.Parse(configuration[$"{SectionDefault}:usestrictlocale"] ?? DoNotUseStrictLocale);
            options.TtlInMinutes = int.Parse(configuration[$"{SectionDefault}:ttlinminutes"] ?? DefaultTtl);
        }
    }
}
