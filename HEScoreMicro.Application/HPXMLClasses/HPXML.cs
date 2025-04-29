
using HEScoreMicro.Application.HPXMLClasses.EnergyStars;
using HEScoreMicro.Application.HPXMLClasses.Systems;
using HEScoreMicro.Application.HPXMLClasses.ZoneFloors;
using HEScoreMicro.Application.HPXMLClasses.ZoneRoofs;
using HEScoreMicro.Application.HPXMLClasses.ZoneWalls;
using System.Xml.Serialization;

namespace HEScoreMicro.Application.HPXMLClasses
{
    [XmlRoot("HPXML", Namespace = "http://hpxmlonline.com/2023/09")]
    public class HPXML
    {
        [XmlAttribute("schemaVersion")]
        public string SchemaVersion { get; set; } = "4.0";
        public XMLTransactionHeaderInformation XMLTransactionHeaderInformation { get; set; }
        public SoftwareInfo SoftwareInfo { get; set; }
        public Contractor Contractor { get; set; }
        [XmlElement("Building")]
        public BuildingHPXML Building { get; set; }
        public Project Project { get; set; }
    }

    public class XMLTransactionHeaderInformation
    {
        public string XMLType { get; set; }
        public string XMLGeneratedBy { get; set; }
        public DateTime CreatedDateAndTime { get; set; } = DateTime.Now;
        public string Transaction { get; set; }
    }

    public class SoftwareInfo
    {
        public string SoftwareProgramUsed { get; set; }
        public string SoftwareProgramVersion { get; set; }
    }

    public class BuildingHPXML
    {
        [XmlElement("BuildingID")]
        public BuildingID BuildingID { get; set; }
        public Site Site { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public BuildingDetails BuildingDetails { get; set; }
        public BuildingExtension extension { get; set; }
    }

    public class BuildingID
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class Site
    {
        [XmlElement("SiteID")]
        public SiteID SiteID { get; set; }
        [XmlElement("Address")]
        public AddressHPXML Address { get; set; }
    }

    public class SiteID
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class AddressHPXML
    {
        public string AddressType { get; set; } = "street";
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityMunicipality { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
    }

    public class ProjectStatus
    {
        public string EventType { get; set; }
        public string Date { get; set; }
    }

    public class BuildingDetails
    {
        public BuildingSummary BuildingSummary { get; set; }
        public GreenBuildingVerifications GreenBuildingVerifications { get; set; }
        public Enclosure Enclosure { get; set; }
        [XmlElement("Systems")]
        public HpxmlSystems Systems { get; set; }
    }
    public class GreenBuildingVerifications
    {
        public GreenBuildingVerification GreenBuildingVerification { get; set; } = new GreenBuildingVerification { };
    }
    public class GreenBuildingVerification
    {
        public SystemIdentifier SystemIdentifier { get; set; } = new SystemIdentifier
        {
            Id = "EnergyStart-1"
        };
        public string? Type { get; set; } = "Home Performance with ENERGY STAR";
    }

    public class BuildingSummary
    {
        [XmlElement("Site")]
        public BSSite Site { get; set; }
        public BuildingConstruction BuildingConstruction { get; set; }
    }

    public class BuildingConstruction
    {
        public int YearBuilt { get; set; }
        public string ResidentialFacilityType { get; set; }
        public int? NumberofUnitsInBuilding { get; set; }
        public double? NumberofConditionedFloorsAboveGrade { get; set; }
        public double? AverageCeilingHeight { get; set; }
        public int NumberofBedrooms { get; set; }
        public double? ConditionedFloorArea { get; set; }
        public double? ConditionedBuildingVolume { get; set; }
        public string? ManufacturedHomeSections { get; set; }
    }


    public class BSSite
    {
        public string? OrientationOfFrontOfHome { get; set; }
        public int? AzimuthOfFrontOfHome { get; set; }
    }

    public class Enclosure
    {
        public AirInfiltration? AirInfiltration { get; set; }
        public Attics? Attics { get; set; }
        public Foundations? Foundations { get; set; }
        public Roofs? Roofs { get; set; }
        public Walls? Walls { get; set; }
        public FoundationWalls? FoundationWalls { get; set; }
        public Floors? Floors { get; set; }
        public Slabs? Slabs { get; set; }
        public Windows? Windows { get; set; }
        public Skylights? Skylights { get; set; }
    }

    public class AirInfiltration
    {
        [XmlElement("AirInfiltrationMeasurement")]
        //public List<AirInfiltrationMeasurement> AirInfiltrationMeasurement { get; set; }
        public AirInfiltrationMeasurement AirInfiltrationMeasurement { get; set; }
        public AirSealing? AirSealing { get; set; }
    }
    public class AirSealing
    {
        public SystemIdentifier SystemIdentifier { get; set; }
    }
    public class AirInfiltrationMeasurement
    {
        [XmlElement("SystemIdentifier")]
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? HousePressure { get; set; }
        public string? LeakinessDescription { get; set; }
        public BuildingAirLeakage BuildingAirLeakage { get; set; }
    }
    public class SystemIdentifier
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
    public class BuildingAirLeakage
    {
        public string? UnitofMeasure { get; set; }
        public double? AirLeakage { get; set; }
    }
    public class Insulation
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public double? AssemblyEffectiveRValue { get; set; }
        [XmlElement("Layer")]
        public List<Layer> Layer { get; set; }
    }
    public class Layer
    {
        public string? InstallationType { get; set; }
        public InsulationMaterial? InsulationMaterial { get; set; }
        public double? NominalRValue { get; set; }
    }
    public class InsulationMaterial
    {
        public string? Batt { get; set; }
        public string? LooseFill { get; set; }
        public string? Rigid { get; set; }
        public string? SprayFoam { get; set; }
        [XmlElement("Other")]
        public Others? Other { get; set; }
        public None? None { get; set; }
        public Unknown? Unknown { get; set; }
    }
    public class Others { }
    public class None { }
    public class Unknown { }

    public class BuildingExtension
    {
        public string? Comments { get; set; }
    }
    public class AttachedToWall
    {
        [XmlAttribute("idref")]
        public string IdRef { get; set; }
    }
    public class AttachedToFoundationWall
    {
        [XmlAttribute("idref")]
        public string IdRef { get; set; }
    }
    public class AttachedToFloor
    {
        [XmlAttribute("idref")]
        public string IdRef { get; set; }
    }
    public class AttachedToSlab
    {
        [XmlAttribute("idref")]
        public string IdRef { get; set; }
    }
    public class AttachedToRoof
    {
        [XmlAttribute("idref")]
        public string IdRef { get; set; }
    }
    public class Other
    {
        public string? Description { get; set; }
    }
}