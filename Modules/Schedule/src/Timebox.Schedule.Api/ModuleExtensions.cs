using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Timebox.Schedule.Application.Interfaces.Repositories;
using Timebox.Schedule.Application.Interfaces.Services;
using Timebox.Schedule.Application.Services;
using Timebox.Schedule.Infrastructure;
using Timebox.Schedule.Infrastructure.Repositories;

namespace Timebox.Schedule.Api
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddScheduleModule(this IServiceCollection services)
        {
            services.ConfigureScheduleModuleInfrastructure();
            
            services.AddSingleton<IScheduleRepository, ScheduleRepository>();
            services.AddSingleton<IScheduleService, ScheduleService>();
            services.AddSingleton<ISchedulerService, SchedulerService>();

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