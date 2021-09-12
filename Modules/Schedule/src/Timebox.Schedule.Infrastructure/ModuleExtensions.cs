using System;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Timebox.Schedule.Infrastructure.Documents;

namespace Timebox.Schedule.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection ConfigureScheduleModuleInfrastructure(this IServiceCollection services)
        {
            services.AddConvey().AddMongo().AddMongoRepository<ScheduleDocument, Guid>("schedules");
            return services;
        }
    }
}