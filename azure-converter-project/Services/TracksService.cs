using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using VideoConverter.Api.Data;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Services {
    public class TracksService : ITracksService {
        private readonly IAsyncRepository<Track> _repository;
        private readonly IMediaStorageService _mediaStorage;
        private readonly IDistributedCache _cache;

        public TracksService (IAsyncRepository<Track> repository, IMediaStorageService mediaStorage, IDistributedCache cache) {
            _repository = repository;
            _mediaStorage = mediaStorage;
            _cache = cache;
        }

        public Task Add (Track track) {
            return _repository.Create (track);
        }

        public async Task<IEnumerable<Track>> Get () {
            IEnumerable<Track> tracks = null;
            var json = await _cache.GetStringAsync ("tracks");
            if (json != null) {
                tracks = JsonConvert.DeserializeObject<IEnumerable<Track>> (json);
            } else {
                tracks = await _repository.Get ();
                json = JsonConvert.SerializeObject (tracks);
                await _cache.SetStringAsync ("tracks", json, new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds (30)
                });
            }
            return tracks;
        }

        public Task<IEnumerable<Track>> Get (Expression<Func<Track, bool>> predicate) {
            return _repository.Get (predicate);
        }

        public Task<Track> Get (string id) {
            return _repository.Find (id);
        }

        public async Task<bool> Remove (string id) {
            var track = await _repository.Find (id);
            if (track != null) {
                await _repository.Remove (track);
                await _cache.RemoveAsync ("tracks");
                return true;
            }
            return false;
        }

        public async Task<Track> Update (Track track) {
            var trackToUpdate = await _repository.Find (track.Id);
            trackToUpdate.Title = track.Title ?? trackToUpdate.Title;
            trackToUpdate.Album = track.Album ?? trackToUpdate.Album;
            trackToUpdate.Artist = track.Artist ?? trackToUpdate.Artist;
            await _repository.Update (trackToUpdate);
            await _cache.RemoveAsync ("tracks");
            return trackToUpdate;
        }

        public async Task<Track> UploadTrackAsync (ConvertedItem converted) {

            Track track = new Track {
                Title = converted.Title,
                VideoUrl = converted.VideoUrl
            };
            track.AudioUrl = await _mediaStorage.Upload (converted.Stream, extension: "mp3");
            await _repository.Create (track);
            await _cache.RemoveAsync ("tracks");
            return track;
        }
    }
}