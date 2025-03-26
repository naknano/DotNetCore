using API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        
        public DbSet<Difficulty> difficulty { get; set; }
        public DbSet<Region> region { get; set; }
        public DbSet<Walk> walk { get; set; }

    }
}
