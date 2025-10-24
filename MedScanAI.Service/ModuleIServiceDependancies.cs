using MedScanAI.Service.Abstracts;
using MedScanAI.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace MedScanAI.Service
{
    public static class ModuleIServiceDependancies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<IConfirmEmailService, ConfirmEmailService>();

            return services;
        }
    }
}
