using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Data {
    public interface IAsyncRepository {
        Task Create (Track track);
        Task Remove (Track track);
        Task Update (Track track);
        Task<Track> Find (int id);
        Task<IEnumerable<Track>> Get ();
        Task<IEnumerable<Track>> Get (Expression<Func<Track, bool>> pred);
    }
}