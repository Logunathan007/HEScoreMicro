
using System.Xml.Serialization;

namespace EnergyScore.Application.Templates.HPXMLs.ZoneFloors
{
    public class Floors
    {
        [XmlElement("Floor")]
        public List<Floor> Floor { get; set; }
    }
    public class Floor
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? Area { get; set; }
        [XmlElement("Insulation")]
        public Insulation Insulation { get; set; }
    }
}
