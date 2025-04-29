using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.ZoneWalls
{
    public class ZoneWallFields : IHasBuildingId, IHasId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public bool? ExteriorWallSame  { get; set; }
    }
    public class ZoneWall : ZoneWallFields
    {
        // Navigation properties
        public ICollection<Wall> Walls { get; set; }
        public Building Building { get; set; }
    }
    public class ZoneWallDTO : ZoneWallFields
    {
        public ICollection<WallDTO> Walls { get; set; }
    }
}
