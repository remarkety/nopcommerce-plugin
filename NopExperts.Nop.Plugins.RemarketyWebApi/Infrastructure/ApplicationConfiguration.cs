using System;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nop.Core.Infrastructure;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class ApplicationConfiguration : IStartupTask
    {
        public void Execute()
        {
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();
                config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new Iso8601DateTimeConverter());
            });
        }

        public int Order => 0;
    }
}