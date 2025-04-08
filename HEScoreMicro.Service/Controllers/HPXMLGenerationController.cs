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
        public IActionResult Get(Guid Id) {
            var result = _hPXMLGenerationOperations.GetHPXMLString(Id);
            if (result.Result.Failed)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result.Result.Data);
        }
        [HttpGet("HPXMLBase64String/{Id}")]
        public IActionResult GetBase64(Guid Id)
        {
            var result = _hPXMLGenerationOperations.GetBase64HPXMLString(Id);
            if (result.Result.Failed)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result.Result.Data);
        }
    }
}
