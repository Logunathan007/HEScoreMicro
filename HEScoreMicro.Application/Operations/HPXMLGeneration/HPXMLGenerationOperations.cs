
using GenericController.Application.Mapper.Reply;

namespace HEScoreMicro.Application.Operations.HPXMLGeneration
{
    public interface IHPXMLGenerationOperations
    {
        Task<ResponseDTO<string>> GetHPXMLString(Guid Id);
        Task<ResponseDTO<string>> GetBase64HPXMLString(Guid Id);
    }
    public class HPXMLGenerationOperations(
        IHPXMLObjectCreation _hPXMLObjectCreation) : IHPXMLGenerationOperations
    {
        public async Task<ResponseDTO<string>> GetHPXMLString(Guid Id)
        {
            var obj = await _hPXMLObjectCreation.CreateHPXMLObject(Id);
            if (obj.Failed)
            {
                return new ResponseDTO<string>
                {
                    Failed = true,
                    Message = "HPXML object creation failed."
                };
            }
            var hpxmlString = await _hPXMLObjectCreation.GenerateHPXMLString(obj.Data);
            if (hpxmlString.Failed)
            {
                return hpxmlString;
            }
            return new ResponseDTO<string>
            {
                Failed = false,
                Message = "HPXML string generated successfully.",
                Data = hpxmlString.Data
            };
        }
        public async Task<ResponseDTO<string>> GetBase64HPXMLString(Guid Id)
        {
            var obj = await GetHPXMLString(Id);
            if (obj.Failed)
            {
                return obj;
            }
            var base64Stringres = await _hPXMLObjectCreation.GenerateHPXMLBase64String(obj.Data);
            if (base64Stringres.Failed)
            {
                return base64Stringres;
            }
            return new ResponseDTO<string>
            {
                Failed = false,
                Message = "HPXML string generated successfully.",
                Data = base64Stringres.Data
            };
        }
    }
}
