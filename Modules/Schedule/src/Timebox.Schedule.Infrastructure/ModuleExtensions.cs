using System;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Timebox.Schedule.Infrastructure.Documents;

namespace Timebox.Schedule.Infrastructure
{
    public static class ModuleExtensions
    {
        public static IServiceCollection ConfigureScheduleModuleInfrastructure(this IServiceCollection services)
        {
            services.AddConvey().AddMongo("mongo-timebox-schedule").AddMongoRepository<ScheduleDocument, Guid>("schedules");
            return services;
        }
    }
}