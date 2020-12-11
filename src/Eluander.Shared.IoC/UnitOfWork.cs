using Eluander.Infra.Identity;
using Eluander.Shared.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Eluander.Shared.IoC
{
    internal class UnitOfWork
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton(NHibernateHelper.SessionFactory());
            services.AddScoped<IUow, Uow>();
        }
    }
}
