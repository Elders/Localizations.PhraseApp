using Localizations.PhraseApp.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Localizations.PhraseApp.Internal
{
    internal class PhraseAppOptionsProvider : OptionsProviderBase<PhraseAppOptions>
    {
        private const string DefaultAddress = "https://api.phraseapp.com/api/v2/";
        private const string DefaultLocale = "en";
        private const string DoNotUseStrictLocale = "false";
        private const string DefaultTtl = "5";

        public const string Section = "localization:phraseapp";

        public PhraseAppOptionsProvider(IConfiguration configuration) : base(configuration) { }

        public override void Configure(PhraseAppOptions options)
        {
            options.Address = configuration[$"{Section}:address"] ?? DefaultAddress;
            options.AccessToken = configuration[$"{Section}:accesstoken"];
            options.ProjectId = configuration[$"{Section}:projectid"];
            options.DefaultLocale = configuration[$"{Section}:defaultlocale"] ?? DefaultLocale;
            options.UseStrictLocale = bool.Parse(configuration[$"{Section}:usestrictlocale"] ?? DoNotUseStrictLocale);
            options.TtlInMinutes = int.Parse(configuration[$"{Section}:ttlinminutes"] ?? DefaultTtl);
        }
    }
}
