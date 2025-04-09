
using System.Xml.Serialization;

namespace HEScoreMicro.Application.HPXMLClasses.Systems
{
    public class HpxmlSystems
    {
        public HVAC? HVAC { get; set; }
        public WaterHeating? WaterHeating { get; set; }
        public Photovoltaics? Photovoltaics { get; set; }
    }
    public class WaterHeating
    {
        [XmlElement]
        public WaterHeatingSystem? WaterHeatingSystem { get; set; }
    }
    public class WaterHeatingSystem
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public string? FuelType { get; set; }
        public string? WaterHeaterType { get; set; }
        public int? YearInstalled { get; set; }
        public int? ModelYear { get; set; }
        public double? FractionDHWLoadServed { get; set; }
        public double? EnergyFactor { get; set; }
        public double? UniformEnergyFactor { get; set; }
    }
    public class Photovoltaics
    {
        [XmlElement]
        public PVSystem? PVSystem { get; set; }
    }
    public class PVSystem
    {
        public SystemIdentifier SystemIdentifier { get; set; }
        public string? ArrayOrientation { get; set; }
        public double? ArrayAzimuth { get; set; }
        public double? ArrayTilt { get; set; }
        public double? MaxPowerOutput { get; set; }
        public double? CollectorArea { get; set; }
        public int? NumberOfPanels { get; set; }
        public int? YearModulesManufactured { get; set; }
        public int? YearInstalled { get; set; }
    }
}
