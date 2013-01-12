using System;
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
    }
}
