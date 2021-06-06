using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Timebox.Schedule.Application.Interfaces.Services;
using Timebox.Schedule.Application.Services;

namespace Timebox.Schedule.Api
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddScheduleModule(this IServiceCollection services)
        {
            services.AddSingleton<IScheduleService, ScheduleService>();

            services.AddControllers()
            .AddApplicationPart(Assembly.GetExecutingAssembly())
            .AddControllersAsServices();
            
            return services;
        }
        
        public static IApplicationBuilder UseScheduleModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}