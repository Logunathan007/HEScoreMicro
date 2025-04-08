
using System.Xml.Serialization;

namespace HEScoreMicro.Application.HPXMLClasses.ZoneRoofs
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
        public AttachedToRoof? AttachedToRoof { get; set; }
        [XmlElement]
        public AttachedToWall? AttachedToWall { get; set; }
        [XmlElement]
        public AttachedToFloor? AttachedToFloor { get; set; }
    }

    public class AtticType
    {
        [XmlElement("Attic")]
        public AtticTypes? Attic { get; set; }
        [XmlElement]
        public CathedralCeiling? CathedralCeiling { get; set; }
        [XmlElement]
        public FlatRoof? FlatRoof { get; set; }
        [XmlElement]
        public BelowApartment? BelowApartment { get; set; }
    }
    public class CathedralCeiling
    {
    }
    public class FlatRoof
    {
    }
    public class BelowApartment
    {
    }
    public class AtticTypes
    {
        public bool? Vented { get; set; }
        public bool? Conditioned { get; set; }
        public bool? CapeCod { get; set; }
    }
}
