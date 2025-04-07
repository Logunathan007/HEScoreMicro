using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Domain.Entity.ZoneRoofAttic;
using HEScoreMicro.Domain.Entity.ZoneWall;
using HEScoreMicro.Domain.Entity.ZoneWindow;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Persistence.MakeConnection
{
    public class DbConnect(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<ZoneFloor> ZoneFloor { get; set; }
        public DbSet<Foundation> Foundation { get; set; }
        public DbSet<ZoneRoof> ZoneRoof { get; set; }
        public DbSet<RoofAttic> RoofAttic { get; set; }
        public DbSet<Wall> Wall { get; set; }
        public DbSet<ZoneWall> ZoneWall { get; set; }
        public DbSet<WaterHeater> WaterHeater { get; set; }
        public DbSet<PVSystem> PVSystem { get; set; }
        public DbSet<Window> Window { get; set; }
        public DbSet<ZoneWindow> ZoneWindow { get; set; }
        public DbSet<DuctLocation> DuctLocation { get; set; }
        public DbSet<HeatingCoolingSystem> HeatingCoolingSystem { get; set; }
        public DbSet<Systems> Systems { get; set; }
    }
}
