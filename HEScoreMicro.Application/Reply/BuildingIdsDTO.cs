using GenericController.Application.Mapper.Reply;

namespace HEScoreMicro.Application.Reply
{
    public class BuildingIdsDTO : Response
    {
        public Guid? BuildingId { get; set; }
        public Guid? AddressId { get; set; }
    }
}
