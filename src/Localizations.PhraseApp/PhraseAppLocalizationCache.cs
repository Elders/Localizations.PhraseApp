using System;
using System.Collections.Concurrent;
using Localizations.PhraseApp.Internal;

namespace Localizations.PhraseApp
{
    public class PhraseAppLocalizationCache
    {
        public PhraseAppLocalizationCache()
        {
            LocaleCache = new ConcurrentDictionary<string, SanitizedPhraseAppLocaleModel>();
            EtagPerLocaleCache = new ConcurrentDictionary<string, string>();
            TranslationCachePerLocale = new ConcurrentDictionary<string, ConcurrentDictionary<string, TranslationModel>>();
        }

        internal ConcurrentDictionary<string, SanitizedPhraseAppLocaleModel> LocaleCache { get; private set; }
        internal ConcurrentDictionary<string, ConcurrentDictionary<string, TranslationModel>> TranslationCachePerLocale { get; private set; }
        internal ConcurrentDictionary<string, string> EtagPerLocaleCache { get; private set; }

        internal DateTime NextCheckForChanges { get; set; }
    }

    public class PhraseAppLocalizationCacheFactory
    {
        private ConcurrentDictionary<string, PhraseAppLocalizationCache> tenantCaches;

        public PhraseAppLocalizationCacheFactory()
        {
            tenantCaches = new ConcurrentDictionary<string, PhraseAppLocalizationCache>();
        }

        public PhraseAppLocalizationCache GetCache(string tenant)
        {
            PhraseAppLocalizationCache cache;

            if (tenantCaches.TryGetValue(tenant, out cache) == false)
            {
                cache = new PhraseAppLocalizationCache();
                tenantCaches.TryAdd(tenant, cache);
            }

            return cache;
        }
    }
}
