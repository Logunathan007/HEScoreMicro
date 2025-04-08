
using System.Xml.Serialization;

namespace EnergyScore.Application.Templates.HPXMLs.ZoneRoofs
{
    public class Attics
    {
        [XmlElement("Attic")]
        public List<Attic> Attic { get; set; }
    }
    public class Attic
    {
        [XmlElement("SystemIdentifier")]
        public SystemIdentifier SystemIdentifier { get; set; }
        [XmlElement("AtticType")]
        public AtticType AtticType { get; set; }
        [XmlElement]
        public List<AttachedToRoof> AttachedToRoof { get; set; }
        [XmlElement]
        public List<AttachedToWall>  AttachedToWall { get; set; }
        [XmlElement]
        public List<AttachedToFloor> AttachedToFloor { get; set; }
    }

    public class AtticType
    {
        [XmlElement("Attic")]
        public AtticTypes? Attic { get; set; }
        [XmlElement]
        public string? CathedralCeiling { get; set; }
        [XmlElement]
        public string? FlatRoof { get; set; }
        [XmlElement]
        public string? BelowApartment { get; set; }
    }
    public class AtticTypes
    {
        public bool? Vented { get; set; }
        public bool? Conditioned { get; set; }
        public bool? CapeCod { get; set; }
    }
}
