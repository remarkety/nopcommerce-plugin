using System.Web.Http;
using AutoMapper;
using Nop.Admin.Infrastructure.Mapper;
using Nop.Core.Infrastructure;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebAdmin;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

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