namespace HEScoreMicro.Domain.Entity.HeatingCoolingSystems
{
    public class DuctLocationFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public string? Location { get; set; }
        public bool? DuctsIsInsulated { get; set; }
    }
    public class DuctLocation : DuctLocationFields
    {
        public Guid SystemsId { get; set; }
        public Systems Systems { get; set; }
        public Building Building { get; set; }
    }
    public class DuctLocationDTO : DuctLocationFields
    {
    }
}
