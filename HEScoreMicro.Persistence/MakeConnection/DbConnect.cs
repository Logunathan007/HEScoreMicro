using HEScoreMicro.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Persistence.MakeConnection
{
    public class DbConnect(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<About> About { get; set; }
    }
}
