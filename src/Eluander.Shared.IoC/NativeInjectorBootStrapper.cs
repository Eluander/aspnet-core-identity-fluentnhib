using Eluander.Infra.Identity;
using Eluander.Shared.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Eluander.Shared.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Connection and transaction
            services.AddSingleton(NHibernateHelper.SessionFactory());
            services.AddScoped<IUow, Uow>();             
        }
    }
}
