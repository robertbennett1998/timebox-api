using System;
using System.Linq;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Application.Interfaces.Repository
{
    public interface IRepository<TModel> where TModel : IEntity
    {
        void Add(TModel model);
        void AddOrUpdate(TModel model);
        void Get(Guid id);
        void Get(Func<TModel, IQueryable<TModel>> selector);
        void Update(TModel model);
        void Update(Guid id, Action<TModel> modifier);
        void Remove(Guid id);
        void Remove(Func<TModel, IQueryable<TModel>> selector);
    }
}