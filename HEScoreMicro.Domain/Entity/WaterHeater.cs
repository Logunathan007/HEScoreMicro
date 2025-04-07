namespace HEScoreMicro.Domain.Entity
{
    public class WaterHeaterFields:IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public string? WaterHeaterType { get; set; }
        public bool? KnowWaterHeaterEnergyFactor { get; set; }
        public string? Unit { get; set; }
        public double? EnergyValue { get; set; }
        public double? YearOfManufacture { get; set; }
    }
    public class WaterHeater : WaterHeaterFields
    {
        public Building Building { get; set; }
    }
    public class WaterHeaterDTO : WaterHeaterFields
    {
    }
}
