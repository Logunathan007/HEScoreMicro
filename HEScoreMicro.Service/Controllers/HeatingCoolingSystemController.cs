using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.HeatingCooling;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeatingCoolingSystemController : ControllerBase
    {
        private readonly IHeatingCoolingSystemOperations _heatingCoolingSystemOperations;
        public HeatingCoolingSystemController(IHeatingCoolingSystemOperations heatingCoolingSystemOperations)
        {
            _heatingCoolingSystemOperations = heatingCoolingSystemOperations;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<HeatingCoolingSystemDTO>>> GetById(Guid Id)
        {
            var res = await _heatingCoolingSystemOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<HeatingCoolingSystemDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _heatingCoolingSystemOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<HeatingCoolingSystemDTO>>>> GetAll()
        {
            var res = await _heatingCoolingSystemOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<HeatingCoolingSystemDTO>>> Post([FromBody] HeatingCoolingSystemDTO heatingCoolingSystemDTO)
        {
            var res = await _heatingCoolingSystemOperations.Add(heatingCoolingSystemDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<HeatingCoolingSystemDTO>>> Put([FromBody] HeatingCoolingSystemDTO heatingCoolingSystemDTO)
        {
            var res = await _heatingCoolingSystemOperations.Update(heatingCoolingSystemDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<HeatingCoolingSystemDTO>>> Delete(Guid Id)
        {
            var res = await _heatingCoolingSystemOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpDelete("Systems/{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> DeleteBySystemsId(Guid Id, [FromKeyedServices("Systems")] ISystemsOperations systemsOperations)
        {
            var res = await systemsOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpDelete("Systems/DuctLocation/DeleteByIds")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> DeleteByDuctLocationIds([FromBody] IEnumerable<Guid> Id, [FromKeyedServices("DuctLocation")] IDuctLocationOperations ductLocationOperations)
        {
            var res = await ductLocationOperations.BulkDelete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
