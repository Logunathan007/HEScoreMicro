

using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.ZoneWindows
{
    public class ZoneWindowFields : IHasId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public double? WindowAreaFront { get; set; }
        public double? WindowAreaBack { get; set; }
        public double? WindowAreaLeft { get; set; }
        public double? WindowAreaRight { get; set; }
        public bool? WindowsSame { get; set; }
    }
    public class ZoneWindow : ZoneWindowFields
    {
        // Navigation properties
        public ICollection<Window> Windows { get; set; }
        public Building Building { get; set; }
    }
    public class ZoneWindowDTO : ZoneWindowFields
    {
        public ICollection<WindowDTO> Windows { get; set; }
    }
}
