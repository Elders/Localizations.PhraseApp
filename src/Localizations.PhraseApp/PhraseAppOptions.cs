using System.ComponentModel.DataAnnotations;

namespace Localizations.PhraseApp
{
    public class PhraseAppOptions
    {
        [Required]
        public string Tenant { get; set; }

        public string Address { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
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
    }
}
