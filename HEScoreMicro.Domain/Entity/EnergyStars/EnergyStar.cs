
using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.EnergyStars
{
    public class EnergyStarFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public bool? EnergyStarPresent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? ContractorBusinessName { get; set; }
        public int? ContractorZipCode { get; set; }
    }
    public class EnergyStar : EnergyStarFields
    {
        //Navigation Property
        public Building Building { get; set; }
    }
    public class EnergyStarDTO : EnergyStarFields
    {
    }
}
