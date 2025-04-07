
namespace HEScoreMicro.Domain.Entity
{
    public class PVSystemFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public bool? HasPhotovoltaic { get; set; }
        public int? YearInstalled { get; set; }
        public string? DirectionPanelsFace { get; set; }
        public string? AnglePanelsAreTilted { get; set; }
        public bool? KnowSystemCapacity { get; set; }
        public int? NumberOfPanels { get; set; }
        public int? DCCapacity { get; set; }
    }
    public class PVSystem : PVSystemFields
    {
        public Building Building { get; set; }
    }
    public class PVSystemDTO : PVSystemFields
    {
    }
}
