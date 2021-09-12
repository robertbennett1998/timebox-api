using System;
using System.Threading.Tasks;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Application.Interfaces.Repositories
{
    public interface IRepository<TModel> where TModel : IEntity
    {
        Task Add(TModel model);
        Task AddOrUpdate(TModel model);
        Task<TModel> Get(Guid id);
        Task<TModel> Get(Func<TModel, bool> selector);
        Task Update(TModel model);
        Task Update(Guid id, Func<TModel, TModel> modifier);
        Task Remove(Guid id);
        Task RemoveAll(Func<TModel, bool> selector);
    }
}