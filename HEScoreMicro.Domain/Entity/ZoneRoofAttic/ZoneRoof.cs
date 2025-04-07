
namespace HEScoreMicro.Domain.Entity.ZoneRoofAttic
{
    public class ZoneRoofFields : IHasBuildingId,IHasId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public bool? EnableSecondRoofAttic { get; set; }
    }
    public class ZoneRoof : ZoneRoofFields
    {
        // Navigation properties
        public ICollection<RoofAttic> RoofAttics { get; set; }
        public Building Building { get; set; }
    }
    public class ZoneRoofDTO : ZoneRoofFields
    {
        public ICollection<RoofAtticDTO> RoofAttics { get; set; }
    }
}
