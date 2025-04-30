
using HEScoreMicro.Domain.Entity.Address;

namespace HEScoreMicro.Domain.Entity.ZoneRoofAttics
{
    public class RoofAtticFields : IHasId
    {
        public Guid Id { get; set; }
        public string? AtticOrCeilingType { get; set; }

        //Roof Assembly
        public string? Construction { get; set; }
        public string? ExteriorFinish { get; set; }
        /*        public double? CathedralCeilingArea { get; set; }
                public int? CathedralCeilingInsulation { get; set; }*/ // same variables are use roofarea & roofinsulation
        public double? RoofArea { get; set; }
        public int? RoofInsulation { get; set; }
        public string? RoofColor { get; set; }
        public double? Absorptance { get; set; }

        //Roof Skylights
        public bool? SkylightsPresent { get; set; }
        public double? SkylightArea { get; set; }
        public bool? SolarScreen { get; set; }
        public bool? KnowSkylightSpecification { get; set; }
        public double? UFactor { get; set; }
        public double? SHGC { get; set; }

        //Skylight Type
        public string? Panes { get; set; }
        public string? FrameMaterial { get; set; }
        public string? GlazingType { get; set; }

        //Attic Floor
        public double? AtticFloorArea { get; set; }
        public int? AtticFloorInsulation { get; set; }

        //Knee Wall
        public bool? KneeWallPresent { get; set; }
        public double? KneeWallArea { get; set; }
        public int? KneeWallInsulation { get; set; }
    }
    public class RoofAttic : RoofAtticFields
    {
        public ZoneRoof ZoneRoof { get; set; }
        public Guid ZoneRoofId { get; set; }
    }
    public class RoofAtticDTO : RoofAtticFields
    {
    }
}
