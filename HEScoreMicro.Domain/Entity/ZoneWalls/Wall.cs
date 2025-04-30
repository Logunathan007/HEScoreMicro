
using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.ZoneWalls
{
    public class WallFields :  IHasId
    {
        public Guid Id { get; set; }
        public string? AdjacentTo { get; set; }
        public string? Construction { get; set; }
        public string? ExteriorFinish { get; set; }
        public int? WallInsulationLevel { get; set; }
    }
    public class Wall : WallFields
    {
        // Navigation properties
        public ZoneWall ZoneWall { get; set; }
        public Guid ZoneWallId { get; set; }
    }
    public class WallDTO : WallFields
    {

    }
}
