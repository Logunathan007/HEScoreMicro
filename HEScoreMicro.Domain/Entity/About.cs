
namespace HEScoreMicro.Domain.Entity
{
    public class AboutFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string? Comments { get; set; }
        public int YearBuilt { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int StoriesAboveGroundLevel { get; set; }
        public int InteriorFloorToCeilingHeight { get; set; }
        public int TotalConditionedFloorArea { get; set; }
        public string DirectionFacedByFrontOfHome { get; set; }
        public bool BlowerDoorTestConducted { get; set; }
        public int? NumberofUnitsInBuilding { get; set; }
        public string? ManufacturedHomeType { get; set; }
        public int? AirLeakageRate { get; set; }
        public bool? AirSealed { get; set; }
        public Guid BuildingId { get; set; }
    }
    public class About : AboutFields
    {
        //Navigation Property
        public Building Building { get; set; }
    }
    public class AboutDTO : AboutFields
    {
    }
}
