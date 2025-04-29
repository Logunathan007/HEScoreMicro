using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity.EnergyStars;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyStarController : ControllerBase
    {
        private readonly IEnergyStarOperations _energyStarOperations;
        public EnergyStarController(IEnergyStarOperations energyStarOperations)
        {
            _energyStarOperations = energyStarOperations;
        }

        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<EnergyStarDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _energyStarOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<EnergyStarDTO>>> Post([FromBody] EnergyStarDTO energyStarDTO)
        {
            var res = await _energyStarOperations.Add(energyStarDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        } 

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<EnergyStarDTO>>> Put([FromBody] EnergyStarDTO energyStarDTO)
        {
            var res = await _energyStarOperations.Update(energyStarDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
