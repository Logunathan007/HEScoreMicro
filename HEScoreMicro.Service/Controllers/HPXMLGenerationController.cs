using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.HPXMLGeneration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HPXMLGenerationController : ControllerBase
    {
        private readonly IHPXMLGenerationOperations _hPXMLGenerationOperations;
        public HPXMLGenerationController(IHPXMLGenerationOperations hPXMLGenerationOperations)
        {
            _hPXMLGenerationOperations = hPXMLGenerationOperations;
        }
        [HttpGet("HPXMLString/{Id}")]
        public async Task<ActionResult<ResponseDTO<string>>> Get(Guid Id) {
            var result = await _hPXMLGenerationOperations.GetHPXMLString(Id);
            if (result.Failed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("HPXMLBase64String/{Id}")]
        public async Task<ActionResult<ResponseDTO<string>>> GetBase64(Guid Id)
        {
            var result = await _hPXMLGenerationOperations.GetBase64HPXMLString(Id);
            if (result.Failed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
