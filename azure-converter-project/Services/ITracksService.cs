using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Services {
    public interface ITracksService {
        Task<Track> UploadTrackAsync (ConvertedItem converted);
        Task<IEnumerable<Track>> Get ();
        Task<Track> Get(int id);
        Task<IEnumerable<Track>> Get (Expression<Func<Track, bool>> predicate);
        Task<bool> Remove(int id);
        Task<Track> Update(Track track);
    }
}