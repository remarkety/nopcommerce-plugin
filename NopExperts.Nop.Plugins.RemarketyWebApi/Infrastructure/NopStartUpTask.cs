using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using NopExperts.Nop.Plugins.RemarketyWebApi.Filters;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class NopStartUpTask : INopStartup
    {
        public void ConfigureServices(IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(EmailTrackingFilterAttribute));
            });
        }

        public void Configure(IApplicationBuilder application)
        {

        }

        public int Order => 0;
    }
}