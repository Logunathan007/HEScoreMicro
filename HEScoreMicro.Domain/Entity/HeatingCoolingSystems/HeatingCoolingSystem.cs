
using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.HeatingCoolingSystems
{
    public class HeatingCoolingSystemFields : IHasId, IHasBuildingId
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public int SystemCount { get; set; }
    }
    public class HeatingCoolingSystem : HeatingCoolingSystemFields
    {
        public ICollection<Systems> Systems { get; set; }
        public Building Building { get; set; }  
    }
    public class HeatingCoolingSystemDTO : HeatingCoolingSystemFields
    {
        public ICollection<SystemsDTO> Systems { get; set; }
    }
}
