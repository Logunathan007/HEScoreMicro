
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using HEScoreMicro.Domain.Entity.ZoneWindows;

namespace HEScoreMicro.Domain.Entity
{
    public class BuildingFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public Guid BuildingId { get; set; } = Guid.Empty;
    }
    public class Building : BuildingFields
    {
        public Address Address { get; set; }
        public About About { get; set; }
        public ZoneFloor ZoneFloor { get; set; }
        public ZoneRoof ZoneRoof { get; set; }
        public ZoneWall ZoneWall { get; set; }
        public ZoneWindow ZoneWindow { get; set; }
        public HeatingCoolingSystem HeatingCoolingSystem { get; set; }
        public WaterHeater WaterHeater { get; set; }
        public PVSystem PVSystem { get; set; }
    }

    public class BuildingDTO : BuildingFields
    {
        public AddressDTO Address { get; set; }
        public AboutDTO About { get; set; }
        public ZoneFloorDTO ZoneFloor { get; set; }
        public ZoneRoofDTO ZoneRoof { get; set; }
        public ZoneWallDTO ZoneWall { get; set; }
        public ZoneWindowDTO ZoneWindow { get; set; }
        public HeatingCoolingSystemDTO HeatingCoolingSystem { get; set; }
        public WaterHeaterDTO WaterHeater { get; set; }
        public PVSystemDTO PVSystem { get; set; }
    }
}
