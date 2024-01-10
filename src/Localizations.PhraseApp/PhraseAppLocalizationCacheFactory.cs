using System.Collections.Concurrent;

namespace Localizations.PhraseApp
{
    public sealed class PhraseAppLocalizationCacheFactory
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
