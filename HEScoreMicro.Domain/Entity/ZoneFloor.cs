namespace HEScoreMicro.Domain.Entity
{
    public class ZoneFloorFields : IHasBuildingId, IHasId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public bool? EnableSecondFoundation { get; set; }
    }
    public class ZoneFloor : ZoneFloorFields
    {
        // Navigation properties
        public ICollection<Foundation> Foundations { get; set; }
        public Building Building { get; set; }
    }
    public class ZoneFloorDTO : ZoneFloorFields
    {
        public ICollection<FoundationDTO> Foundations { get; set; }
    }
    public class FoundationFields : IHasBuildingId, IHasId
    {
        public Guid Id { get; set; }
        public string? FoundationType { get; set; }
        public int? FoundationArea { get; set; }
        public int? SlabInsulationLevel { get; set; }
        public int? FloorInsulationLevel { get; set; }
        public int? FoundationwallsInsulationLevel { get; set; }
        public Guid BuildingId { get; set; }
    }
    public class Foundation : FoundationFields
    {
        // Navigation properties
        public ZoneFloor ZoneFloor { get; set; }
        public Guid ZoneFloorId { get; set; }
    }
    public class FoundationDTO : FoundationFields
    {

    }
}
