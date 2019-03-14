using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SignalrServerDemo.Identity {
    public class ApplicationDbContext : IdentityDbContext {
        public new DbSet<ApplicationUser> Users { get; set; }

        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> opt) : base (opt) { 
           
        }
    }
}