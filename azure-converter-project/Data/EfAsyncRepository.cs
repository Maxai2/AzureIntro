using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Data {
    public class EfAsyncRepository<TEntity> 
        : IAsyncRepository<TEntity> where TEntity : ModelBase, new () {
        private readonly DbContext _db;
        public EfAsyncRepository (DbContext db) {
            _db = db;
        }
        public Task Create (TEntity entity) {
            _db.Set<TEntity> ().Add (entity);
            return _db.SaveChangesAsync ();
        }
        public Task Remove (TEntity entity) {
            _db.Set<TEntity> ().Remove (entity);
            return _db.SaveChangesAsync ();
        }
        public Task Update (TEntity entity) {
            _db.Set<TEntity> ().Update (entity);
            return _db.SaveChangesAsync ();
        }
        public Task<TEntity> Find (string id) {
            return _db.Set<TEntity> ().FirstOrDefaultAsync (t => t.Id == id);
        }
        public async Task<IEnumerable<TEntity>> Get () {
            return await _db.Set<TEntity> ().ToListAsync ();
        }
        public async Task<IEnumerable<TEntity>> Get (Expression<Func<TEntity, bool>> pred) {
            return await _db.Set<TEntity> ().Where (pred).ToListAsync ();
        }
    }
}