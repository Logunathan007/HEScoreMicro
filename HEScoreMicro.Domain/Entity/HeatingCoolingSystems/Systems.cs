

namespace HEScoreMicro.Domain.Entity.HeatingCoolingSystems
{
    public class SystemsFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public double PercentAreaServed { get; set; }
        //Heating
        public string? HeatingSystemType { get; set; }
        public bool? KnowHeatingEfficiency { get; set; }
        public double? HeatingSystemEfficiencyValue { get; set; }
        public int? HeatingSystemYearInstalled { get; set; }
        //Cooling
        public string? CoolingSystemType { get; set; }
        public bool? KnowCoolingEfficiency { get; set; }
        public double? CoolingSystemEfficiencyValue { get; set; }
        public string? CoolingSystemEfficiencyUnit { get; set; }
        public int? CoolingSystemYearInstalled { get; set; }
        //Ducts
        public bool? DuctLeakageTestPerformed { get; set; }
        public bool? DuctAreProfessionallySealed { get; set; }
        public int? DuctLocationCount { get; set; }
    }
    public class Systems : SystemsFields
    {
        // Navigation properties
        public ICollection<DuctLocation> DuctLocations { get; set; } = new List<DuctLocation>();
        public Building? Building { get; set; }
    }

    public class SystemsDTO : SystemsFields
    {
        public ICollection<DuctLocationDTO> DuctLocations { get; set; }
    }
}