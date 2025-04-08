
namespace HEScoreMicro.Domain.Entity.ZoneWalls
{
    public class WallFields : IHasBuildingId, IHasId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public string? Construction { get; set; }
        public string? ExteriorFinish { get; set; }
        public int? WallInsulationLevel { get; set; }
    }
    public class Wall : WallFields
    {
        // Navigation properties
        public ZoneWall ZoneWall { get; set; }
        public Guid ZoneWallId { get; set; }
        public Building Building { get; set; }
    }
    public class WallDTO : WallFields
    {

    }
}
