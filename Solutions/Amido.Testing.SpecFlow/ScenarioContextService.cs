using System;
using Amido.Testing.SpecFlow.Http;
using TechTalk.SpecFlow;

namespace Amido.Testing.SpecFlow
{
    public static class ScenarioContextService
    {
        public static TContext GetContext<TContext>()
        {
            if (!ScenarioContext.Current.ContainsKey(typeof(TContext).FullName))
            {
                var context = Activator.CreateInstance<TContext>();
                ScenarioContext.Current.Set(context);
                return context;
            }

            return ScenarioContext.Current.Get<TContext>();
        }

        public static TContext GetContext<TContext>(string contextKey)
        {
            if (!ScenarioContext.Current.ContainsKey(contextKey))
            {
                var context = Activator.CreateInstance<TContext>();
                ScenarioContext.Current.Set(context, contextKey);
                return context;
            }

            return ScenarioContext.Current.Get<TContext>(contextKey);
        }

        public static ScenarioResponseDictionary GetScenarioResponseDictionary()
        {
            return GetContext<ScenarioResponseDictionary>();
        }
    }
}
