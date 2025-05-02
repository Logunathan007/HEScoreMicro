using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.ZoneWindows
{
    public class WindowFields : IHasId
    {
        public Guid Id { get; set; }
        public int Facing { get; set; } // 0 = North, 1 = West, 2 = South, 3 = East
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
    }
    public class WindowDTO : WindowFields
    {
    }
}
