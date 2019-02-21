using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Data {
    public class EfAsyncRepository : IAsyncRepository {
        private readonly TracksDbContext _db;
        public EfAsyncRepository (TracksDbContext db) {
            _db = db;
        }
        public async Task Create (Track track) {
            await _db.Tracks.AddAsync (track);
            await _db.SaveChangesAsync ();
        }
        public async Task Remove (Track track) {
            _db.Tracks.Remove (track);
            await _db.SaveChangesAsync ();
        }
        public async Task Update (Track track) {
            _db.Tracks.Update (track);
            await _db.SaveChangesAsync ();
        }
        public async Task<Track> Find (int id) {
            return await _db.Tracks
                .FirstOrDefaultAsync (t => t.Id == id);
        }
        public async Task<IEnumerable<Track>> Get () {
            return await _db.Tracks.ToListAsync ();
        }
        public async Task<IEnumerable<Track>> Get (Expression<Func<Track, bool>> pred) {
            return await _db.Tracks.Where (pred).ToListAsync ();
        }
    }
}