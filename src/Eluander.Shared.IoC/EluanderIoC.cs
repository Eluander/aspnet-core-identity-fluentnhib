using Microsoft.Extensions.DependencyInjection;

namespace Eluander.Shared.IoC
{
    public sealed class EluanderIoC
    {
        public static void Register(IServiceCollection services)
        {
            ApplicationService.Register(services);
            UnitOfWork.Register(services);
        }
    }
}
