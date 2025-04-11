using GenericController.Application.Mapper.Reply;

namespace HEScoreMicro.Application.Reply
{

    public class ValidationDTO : Response
    {
        public List<Object> Errors { get; set; }
        public string? HomeJson { get; set; }
    }
}
