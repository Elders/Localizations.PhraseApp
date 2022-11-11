using System.Collections.Generic;

namespace Localizations.PhraseApp
{
    public class PhraseAppOptions
    {
        public PhraseAppOptions()
        {
            TenantOptions = new List<PhraseAppOptions>();
        }

        public string Address { get; set; }

        public string AccessToken { get; set; }

        public string ProjectId { get; set; }
        /// <summary>
        /// Specifies default fall back locale
        /// </summary>
        public string DefaultLocale { get; set; }

        /// <summary>
        /// Specifies if fall back to two letter part of locale is allowed e.g en-GB would fall back to en
        /// </summary>
        public bool UseStrictLocale { get; set; }

        public int TtlInMinutes { get; set; }

        public List<PhraseAppOptions> TenantOptions { get; set; }
    }
}
