using Autofac;
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
