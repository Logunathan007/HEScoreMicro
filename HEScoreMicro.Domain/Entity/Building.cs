
namespace HEScoreMicro.Domain.Entity
{
    public class BuildingFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public Guid? BuildingId { get; set; }
    }
    public class Building : BuildingFields
    {
        public Address Address { get; set; }
    }

    public class BuildingDTO : BuildingFields
    {
    }
}
