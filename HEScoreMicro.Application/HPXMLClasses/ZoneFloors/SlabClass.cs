using System.Xml.Serialization;

namespace EnergyScore.Application.Templates.HPXMLs.ZoneFloors
{
    public class Slabs
    {
        [XmlElement("Slab")]
        public List<Slab> Slab { get; set; }
    }
    public class Slab
    {
        [XmlElement("SystemIdentifier")]
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? Area { get; set; }
        public double? ExposedPerimeter { get; set; }
        [XmlElement("PerimeterInsulation")]
        public PerimeterInsulation PerimeterInsulation { get; set; }
    }
    public class PerimeterInsulation
    {
        [XmlElement("SystemIdentifier")]
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? AssemblyEffectiveRValue { get; set; }
        [XmlElement("Layer")]
        public List<Layer>? Layer { get; set; }
    }
}
