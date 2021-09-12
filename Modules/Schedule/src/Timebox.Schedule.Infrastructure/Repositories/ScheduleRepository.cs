using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Timebox.Schedule.Application.Interfaces.Repositories;
using Timebox.Schedule.Domain.Entities;
using Timebox.Schedule.Infrastructure.Documents;

namespace Timebox.Schedule.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IMongoRepository<ScheduleDocument, Guid> _scheduleRepository;
        
        public ScheduleRepository(IMongoRepository<ScheduleDocument, Guid> scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        
        public async Task Add(ISchedule model)
        {
            await _scheduleRepository.AddAsync(ScheduleDocument.FromEntity(model));
        }

        public async Task AddOrUpdate(ISchedule model)
        {
            if (await _scheduleRepository.ExistsAsync(schedule => schedule.Id == model.Id))
                await _scheduleRepository.UpdateAsync(ScheduleDocument.FromEntity(model));
            else
                await _scheduleRepository.AddAsync(ScheduleDocument.FromEntity(model));
        }

        public async Task<ISchedule> Get(Guid id)
        {
            return ScheduleDocument.ToEntity(await _scheduleRepository.GetAsync(id));
        }

        public async Task<ISchedule> Get(Func<ISchedule, bool> selector)
        {
            return ScheduleDocument.ToEntity(
                await _scheduleRepository.GetAsync(document => selector(ScheduleDocument.ToEntity(document))));
        }

        public async Task Update(ISchedule model)
        {
            await _scheduleRepository.UpdateAsync(ScheduleDocument.FromEntity(model));
        }

        public async Task Update(Guid id, Func<ISchedule, ISchedule> modifier)
        {
            var schedule = await Get(id);

            if (schedule == null)
            {
                return;
            }
            
            await Update(modifier(schedule));
        }

        public async Task Remove(Guid id)
        {
            await _scheduleRepository.DeleteAsync(id);
        }

        public async Task RemoveAll(Func<ISchedule, bool> selector)
        {
            await _scheduleRepository.DeleteAsync(document => selector(ScheduleDocument.ToEntity(document)));
        }

        public async Task<IEnumerable<ISchedule>> GetAll()
        {
            var schedules = await _scheduleRepository.Collection.FindAsync(FilterDefinition<ScheduleDocument>.Empty);
            return schedules.ToEnumerable().Select(ScheduleDocument.ToEntity);
        }
    }
}