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
        /// Specifies default fall back locale. Default is en.
        /// </summary>
        public string DefaultLocale { get; set; }

        /// <summary>
        /// Specifies if fall back to two letter part of locale is allowed e.g en-GB would fall back to en. Default is false.
        /// </summary>
        public bool UseStrictLocale { get; set; }

        /// <summary>
        /// Default is 5 min.
        /// </summary>
        public int TtlInMinutes { get; set; }
    }
}
