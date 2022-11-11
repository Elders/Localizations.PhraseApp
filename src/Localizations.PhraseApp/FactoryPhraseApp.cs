//using System.Collections.Concurrent;

//namespace Localizations.PhraseApp
//{
//    public class FactoryPhraseApp
//    {
//        ConcurrentDictionary<string, ILocalization> store;

//        public FactoryPhraseApp()
//        {
//            store = new ConcurrentDictionary<string, ILocalization>();
//        }

//        public ILocalization GetService(string tenant)
//        {
//            if(store.TryGetValue(tenant, out ILocalization localization))
//            {
//                return localization;
//            }
//            var services = Initialize(tenant);
//            return services;
//        }

//        public ILocalization Initialize(string tenant)
//        {
//            //TODO: implement this

//            return store.TryAdd(tenant, ILocalization localization);

//        }

//    }
//}

