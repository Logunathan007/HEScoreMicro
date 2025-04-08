
using System.Xml.Serialization;

namespace HEScoreMicro.Application.HPXMLClasses.ZoneRoofs
{
    public class Roofs
    {
        [XmlElement("Roof")]
        public List<Roof> Roof { get; set; }
    }

    public class Roof
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? Area { get; set; }
        public string? RoofType { get; set; }
        public string? RoofColor { get; set; }
        public double? SolarAbsorptance { get; set; }
        public bool? RadiantBarrier { get; set; }
        [XmlElement("Insulation")]
        public Insulation Insulation { get; set; }
    }
}
