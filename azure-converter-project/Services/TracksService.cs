using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VideoConverter.Api.Data;
using VideoConverter.Api.Models;
// using StackExchange.Redis;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;

namespace VideoConverter.Api.Services {
    public class TracksService : ITracksService {
         private readonly IAsyncRepository _repository;   
        private readonly IMediaStorageService _mediaStorage;     
        private readonly IDistributedCache _cache;
        // private readonly ConnectionMultiplexer _redisConnection;

        public TracksService (IAsyncRepository repository, IMediaStorageService mediaStorage, IDistributedCache cache) {
            _repository = repository;
            _mediaStorage = mediaStorage;
            _cache = cache;
            // asdasd
            // _redisConnection = ConnectionMultiplexer.Connect("musicPlayer94.redis.cache.windows.net:6380,password=7FCqXtQ0U6q2yTS8xYgdAQ8U0kr87Y+g2Z9kIwPr2XI=,ssl=True,abortConnect=False");
        }

        // public async Task<IEnumerable<Track>> Get () {
        //     var db = _redisConnection.GetDatabase();
        //     IEnumerable<Track> tracks = null;

        //     if (db.KeyExists("tracks")) {
        //         tracks = JsonConvert.DeserializeObject<IEnumerable<Track>>(db.StringGet("tracks"));
        //     } else {
        //         tracks = await _repository.Get();
        //         var json = JsonConvert.SerializeObject(tracks);
        //         db.StringSet("tracks", json, TimeSpan.FromSeconds(30));
        //     }

        //     // db.StringSet("", "");
        //     // db.StringGet("");
        //     return tracks;
        // }

        public async Task<IEnumerable<Track>> Get () {
            IEnumerable<Track> tracks = null;
            var json = await _cache.GetStringAsync("tracks");

            if (json != null) {
                tracks = JsonConvert.DeserializeObject<IEnumerable<Track>>(json);
            } else {
                tracks = await _repository.Get();
                json = JsonConvert.SerializeObject(tracks);
                var options = new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };
                await _cache.SetStringAsync("tracks", json, options);
            }

            // db.StringSet("", "");
            // db.StringGet("");
            return tracks;
        }

        public Task<IEnumerable<Track>> Get (Expression<Func<Track, bool>> predicate) {
            return _repository.Get (predicate);
        }

        public async Task<Track> Get (int id) {
            return await _repository.Find (id);
        }

        public async Task<bool> Remove (int id) {
            var track = await _repository.Find (id);
            if (track != null) {
                await _repository.Remove (track);
                await _cache.RemoveAsync("tracks");
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
            await _cache.RemoveAsync("tracks");
            return trackToUpdate;
        }

        public async Task<Track> UploadTrackAsync (ConvertedItem converted) {
            Track track = new Track {
                Title = converted.Title,
                VideoUrl = converted.VideoUrl
            };
            track.AudioUrl = await _mediaStorage.Upload (converted.Stream, extension: "mp3");
            await _repository.Create (track);
            await _cache.RemoveAsync("tracks");
            return track;
        }
    }
}