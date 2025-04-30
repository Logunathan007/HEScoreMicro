
namespace HEScoreMicro.Domain.Entity.HeatingCoolingSystems
{
    public class DuctLocationFields : IHasId
    {
        public Guid Id { get; set; }
        public string? Location { get; set; }
        public double? PercentageOfDucts { get; set; }
        public bool? DuctsIsInsulated { get; set; }
    }
    public class DuctLocation : DuctLocationFields
    {
        // Navigation properties
        public Guid SystemsId { get; set; }
        public Systems Systems { get; set; }
    }
    public class DuctLocationDTO : DuctLocationFields
    {
    }
}
