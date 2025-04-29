namespace HEScoreMicro.Domain.Entity
{
    public interface IHasId
    {
        public Guid Id { get; set; }
    }
    public interface IHasBuildingId
    {
        public Guid BuildingId { get; set; }
    }
}
