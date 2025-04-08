
using System.Xml.Serialization;

namespace EnergyScore.Application.Templates.HPXMLs.ZoneFloors
{
    public class Foundations
    {
        [XmlElement("Foundation")]
        public List<Foundation> Foundation { get; set; }
    }
    public class Foundation
    {
        [XmlElement("SystemIdentifier")]
        public SystemIdentifier SystemIdentifier { get; set; }
        public FoundationType FoundationType { get; set; }
        [XmlElement("AttachedToFoundationWall")]
        public List<AttachedToFoundationWall> AttachedToFoundationWall { get; set; }
        [XmlElement("AttachedToFloor")]
        public List<AttachedToFloor> AttachedToFloor { get; set; }
        [XmlElement("AttachedToSlab")]
        public List<AttachedToSlab> AttachedToSlab { get; set; }
    }
    public class FoundationType
    {
        public Basement? Basement { get; set; } = null;
        public Crawlspace? Crawlspace { get; set; } = null;
        public SlabOnGrade? SlabOnGrade { get; set; } = null;
        public Garage? Garage { get; set; } = null;
        public AboveApartment? AboveApartment { get; set; } = null;
        public Ambient? Ambient { get; set; } = null;
    }
    public class Basement
    {
        public bool? Finished { get; set; }
        public bool? Conditioned { get; set; }
    }
    public class Crawlspace
    {
        public bool? Vented { get; set; }
        public bool? Conditioned { get; set; }
    }
    public class SlabOnGrade { }
    public class Garage
    {
        public bool? Conditioned { get; set; }
    }
    public class AboveApartment { }
    public class Ambient { }

}
