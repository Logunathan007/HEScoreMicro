
using System.Xml.Serialization;

namespace HEScoreMicro.Application.HPXMLClasses.Systems;

public class HVAC
{
    public HVACPlant? HVACPlant { get; set; }
    [XmlElement]
    public List<HVACDistribution>? HVACDistribution { get; set; }
}
public class HVACPlant
{
    [XmlElement]
    public List<HeatingSystem>? HeatingSystem { get; set; }
    [XmlElement]
    public List<CoolingSystem>? CoolingSystem { get; set; }
    [XmlElement]
    public List<HeatPump>? HeatPump { get; set; }
}
public class HeatingSystem
{
    public SystemIdentifier SystemIdentifier { get; set; }
    public int? YearInstalled { get; set; }
    public int? ModelYear { get; set; }
    public DistributionSystem? DistributionSystem { get; set; }
    public HeatingSystemType HeatingSystemType { get; set; }
    public string? HeatingSystemFuel { get; set; }
    public double? HeatingCapacity { get; set; }
    public AnnualHeatingEfficiency? AnnualHeatingEfficiency { get; set; }
    public double? FractionHeatLoadServed { get; set; }
    public double? FloorAreaServed { get; set; }
}
public class HeatingSystemType
{
    public Furnace? Furnace { get; set; }
    public WallFurnace? WallFurnace { get; set; }
    public FloorFurnace? FloorFurnace { get; set; }
    public Boiler? Boiler { get; set; }
    public ElectricResistance? ElectricResistance { get; set; }
    public Fireplace? Fireplace { get; set; }
    public Stove? Stove { get; set; }
    public SpaceHeater? SpaceHeater { get; set; }
    public SolarThermal? SolarThermal { get; set; }
    public DistrictSteam? DistrictSteam { get; set; }
}
public class Furnace { }
public class WallFurnace { }
public class FloorFurnace { }
public class Boiler { }
public class ElectricResistance { }
public class Fireplace { }
public class Stove { }
public class SpaceHeater { }
public class SolarThermal { }
public class DistrictSteam { }
public class AnnualHeatingEfficiency
{
    public string? Units { get; set; }
    public double? Value { get; set; }
}
public class AnnualCoolingEfficiency
{
    public string? Units { get; set; }
    public double? Value { get; set; }
}
public class DistributionSystem
{
    [XmlAttribute("idref")]
    public string? IdRef { get; set; }
}
public class CoolingSystem
{
    public SystemIdentifier SystemIdentifier { get; set; }
    public int? YearInstalled { get; set; }
    public int? ModelYear { get; set; }
    public DistributionSystem? DistributionSystem { get; set; }
    public string? CoolingSystemType { get; set; }
    public double? CoolingCapacity { get; set; }
    public double? FractionCoolLoadServed { get; set; }
    public double? FloorAreaServed { get; set; }
    public AnnualCoolingEfficiency AnnualCoolingEfficiency { get; set; }
}
public class HeatPump
{
    public SystemIdentifier SystemIdentifier { get; set; }
    public int? YearInstalled { get; set; }
    public int? ModelYear { get; set; }
    public DistributionSystem? DistributionSystem { get; set; }
    public string? HeatPumpType { get; set; }
    public double? HeatingCapacity { get; set; }
    public double? HeatingCapacity17F { get; set; }
    public double? CoolingCapacity { get; set; }
    public double? FractionHeatLoadServed { get; set; }
    public double? FractionCoolLoadServed { get; set; }
    public double? FloorAreaServed { get; set; }
    public AnnualCoolingEfficiency? AnnualCoolingEfficiency { get; set; }
    public AnnualHeatingEfficiency? AnnualHeatingEfficiency { get; set; }
}
public class HVACDistribution
{
    public SystemIdentifier SystemIdentifier { get; set; }
    public DistributionSystemType DistributionSystemType { get; set; }
    public HVACDistributionImprovement HVACDistributionImprovement { get; set; }
}
public class HVACDistributionImprovement
{
    public bool? DuctSystemSealed { get; set; }
}
public class DistributionSystemType
{
    public AirDistribution? AirDistribution { get; set; }
}
public class AirDistribution
{
    public DuctLeakageMeasurement? DuctLeakageMeasurement { get; set; }
    [XmlElement]
    public List<Ducts>? Ducts { get; set; }
}
public class Ducts
{
    public SystemIdentifier SystemIdentifier { get; set; }
    [XmlElement("DuctInsulationMaterial")]
    public InsulationMaterial? DuctInsulationMaterial { get; set; }
    public double? DuctInsulationRValue { get; set; }
    public double? DuctInsulationThickness { get; set; }
    public string? DuctLocation { get; set; }
    public double? FractionDuctArea { get; set; }
}
public class DuctLeakageMeasurement
{
    public string? DuctType { get; set; }
    public string? LeakinessObservedVisualInspection { get; set; }
    public DuctLeakage? DuctLeakage { get; set; }
}
public class DuctLeakage
{
    public string? Units { get; set; }
    public double? Value { get; set; }
    public string? TotalOrToOutside { get; set; }
}                
