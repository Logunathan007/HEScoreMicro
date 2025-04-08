
using EnergyScore.Application.Templates.HPXMLs.ZoneWalls;
using System.Xml.Serialization;

namespace EnergyScore.Application.Templates.HPXMLs.ZoneRoofs
{
    public class Skylights
    {
        [XmlElement]
        public List<Skylight> Skylight { get; set; }
    }
    public class Skylight
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? Area { get; set; }
        public FrameType FrameType { get; set; }
        public string? GlassLayers { get; set; }
        public string? GlassType { get; set; }
        public string? GasFill { get; set; }
        public double? UFactor { get; set; }
        public double? SHGC { get; set; }
        public ExteriorShading? ExteriorShading { get; set; }
        public StormWindow? StormWindow { get; set; }
        [XmlElement]
        public AttachedToRoof AttachedToRoof { get; set; }
    }
}
