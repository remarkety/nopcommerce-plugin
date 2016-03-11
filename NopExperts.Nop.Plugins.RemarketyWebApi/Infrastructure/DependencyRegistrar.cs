using System.Net.Http.Formatting;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //builder.RegisterType<EfRepository<NewsletterEntity>>()
            //     .As<IRepository<NewsletterEntity>>()
            //     .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
            //     .InstancePerLifetimeScope();
            
        }

        public int Order => 0;
    }
}
