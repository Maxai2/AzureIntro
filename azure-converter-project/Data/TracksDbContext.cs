using Microsoft.EntityFrameworkCore;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Data {
    public class TracksDbContext : DbContext {
        public TracksDbContext (DbContextOptions<TracksDbContext> opt)
         : base (opt) { }

        public DbSet<Track> Tracks { get; set; }
    }
}