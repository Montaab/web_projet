using Service_ERP.IService;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

 
 

namespace Service_ERP
{
    public static class InjectService
    {
        internal static IServiceCollection AddAllService(this IServiceCollection services)
        {

            var allProviderTypes = Assembly.GetAssembly(typeof(Service_ERP.IService.IArticleService))
             .GetTypes().Where(t => t.Namespace != null).ToList();
            foreach (var intfc in allProviderTypes.Where(t => t.IsInterface && t.Name.EndsWith("Service")))
            {
                var impl = allProviderTypes.FirstOrDefault(c => c.IsClass && intfc.Name.Substring(1) == c.Name);
                if (impl != null) services.AddTransient(intfc, impl);
            }
            return services;
        }
    }
}
