
using System.Xml.Serialization;

namespace HEScoreMicro.Application.HPXMLClasses.EnergyStars
{
    public class Project
    {
        public ProjectID ProjectID { get; set; } = new ProjectID();
        public PreBuildingID PreBuildingID { get; set; } = new PreBuildingID();
        public PostBuildingID PostBuildingID { get; set; } = new PostBuildingID();
        public ProjectDetails ProjectDetails { get; set; }
    }
    public class ProjectID
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = "project-1";
    }
    public class PreBuildingID
    {
    }
    public class PostBuildingID
    {
    }
    public class ProjectDetails
    {
        public string? StartDate { get; set; }
        public string? CompleteDateActual { get; set; }
    }
    public class Contractor
    {
        public ContractorDetails ContractorDetails { get; set; }
    }
    public class ContractorDetails
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
    }
    public class BusinessInfo
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public string BusinessName { get; set; }
        [XmlElement("extension")]
        public BusinessInfoextension? extension { get; set; }
    }
    public class BusinessInfoextension
    {
        public string ZipCode { get; set; }
    }
}
