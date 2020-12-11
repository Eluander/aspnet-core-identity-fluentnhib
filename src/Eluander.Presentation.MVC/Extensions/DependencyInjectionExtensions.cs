using Eluander.Shared.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Eluander.Presentation.MVC.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            EluanderIoC.Register(services);
        }
    }
}
