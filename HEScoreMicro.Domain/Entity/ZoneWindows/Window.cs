using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.ZoneWindows
{
    public class WindowFields : IHasBuildingId,IHasId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public bool? SolarScreen { get; set; }
        public bool? KnowWindowSpecification { get; set; }
        public double? UFactor { get; set; }
        public double? SHGC { get; set; }
        public string? Panes { get; set; }
        public string? FrameMaterial { get; set; }
        public string? GlazingType { get; set; }
    }
    public class Window : WindowFields
    {
        // Navigation properties
        public ZoneWindow ZoneWindow { get; set; }
        public Guid ZoneWindowId { get; set; }
        public Building Building { get; set; }
    }
    public class WindowDTO : WindowFields
    {
    }
}
