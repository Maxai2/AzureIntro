using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Data {
    public interface IAsyncRepository<TEntity> where TEntity : ModelBase, new() {
        Task Create (TEntity entity);
        Task Remove (TEntity entity);
        Task Update (TEntity entity);
        Task<TEntity> Find (string id);
        Task<IEnumerable<TEntity>> Get ();
        Task<IEnumerable<TEntity>> Get (Expression<Func<TEntity, bool>> pred);
    }
}