using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Timebox.Task.Application;
using Timebox.Task.Application.Interfaces.Services;
using Timebox.Task.Application.Services;

namespace Timebox.Task.Api
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddTaskModule(this IServiceCollection services)
        {
            services.AddSingleton<ITaskService, TaskService>();
            
            services.AddControllers()
            .AddApplicationPart(Assembly.GetExecutingAssembly())
            .AddControllersAsServices();
            
            return services;
        }
        
        public static IApplicationBuilder UseTaskModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}