
using System.Xml.Serialization;

namespace EnergyScore.Application.Templates.HPXMLs.ZoneFloors
{
    public class FoundationWalls
    {
        [XmlElement("FoundationWall")]
        public List<FoundationWall> FoundationWall { get; set; }
    }
    public class FoundationWall
    {
        [XmlElement("SystemIdentifier")]
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? Area { get; set; }
        [XmlElement("Insulation")]
        public Insulation Insulation { get; set; }
    }
}
