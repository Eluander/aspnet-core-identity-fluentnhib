using Eluander.Infra.Identity;
using Eluander.Infra.Identity.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Eluander.Infra.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Connection and transaction
            services.AddSingleton(NHibernateHelper.SessionFactory());
            services.AddScoped<IUow, Uow>();
        }
    }
}
