using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Application.Operations.HPXMLGeneration;
using HEScoreMicro.Application.Reply;
using HEScoreMicro.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HPXMLGenerationController : ControllerBase
    {
        private readonly IHPXMLGenerationOperations _hPXMLGenerationOperations;
        private readonly IBuildingOperations _buildingOperations;
        public HPXMLGenerationController(IHPXMLGenerationOperations hPXMLGenerationOperations)
        {
            _hPXMLGenerationOperations = hPXMLGenerationOperations;
        }
        [HttpGet("HPXMLString/{Id}")]
        public async Task<ActionResult<ResponseDTO<string>>> Get(Guid Id)
        {
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
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ValidationDTO>> ValidateInputs(Guid Id)
        {
            var result = await _hPXMLGenerationOperations.ValidateInputs(Id);
            /*if(result.Failed)
            {
                return BadRequest(result);
            }*/
            return Ok(result);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<BuildingDTO>>> ClearOldPDF(Guid Id)
        {
            var result = await _hPXMLGenerationOperations.ClearOldPdfNumber(Id);
            if (result.Failed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponseDTO<string>>> GeneratePdf(Guid Id)
        {
            var result = await _hPXMLGenerationOperations.GeneratePDF(Id);
            if (result.Failed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<string>>> SubmitInputs(Guid Id)
        {
            var result = await _hPXMLGenerationOperations.SubmitInputs(Id);
            if (result.Failed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
