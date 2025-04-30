namespace HEScoreMicro.Domain.Entity.Address
{
    public class AddressFields : IHasId
    {
        public Guid Id { get; set; }
        public string? DwellingUnitType { get; set; }
        public string? StreetAddress { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int ZipCode { get; set; }
        public string? AssessmentType { get; set; }
        public Guid BuildingId { get; set; }
    }
    public class Address : AddressFields
    {
        //Navigation Property
        public Building Building { get; set; }
    }
    public class AddressDTO : AddressFields
    {
        public Guid? BuildingId { get; set; }
    }
}
