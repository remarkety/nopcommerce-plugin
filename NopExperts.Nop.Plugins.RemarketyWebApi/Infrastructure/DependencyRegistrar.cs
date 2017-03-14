using System.Net.Http.Formatting;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nop.Admin.Infrastructure.Mapper;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using NopExperts.Nop.Plugins.RemarketyWebApi.Filters;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebAdmin;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            FilterProviders.Providers.Add(new FilterProvider());
        }

        public int Order => 0;
    }
}
