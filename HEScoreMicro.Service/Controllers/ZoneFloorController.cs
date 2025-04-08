using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.ZoneFloors;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneFloorController : ControllerBase
    {
        private readonly IZoneFloorOperations _zoneFloorOperations;
        public ZoneFloorController(IZoneFloorOperations zoneFloorOperations)
        {
            _zoneFloorOperations = zoneFloorOperations;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneFloorDTO>>> GetById(Guid Id)
        {
            var res = await _zoneFloorOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneFloorDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _zoneFloorOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<ZoneFloorDTO>>>> GetAll()
        {
            var res = await _zoneFloorOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<ZoneFloorDTO>>> Post([FromBody] ZoneFloorDTO zoneFloorDTO)
        {
            var res = await _zoneFloorOperations.Add(zoneFloorDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<ZoneFloorDTO>>> Put([FromBody] ZoneFloorDTO zoneFloorDTO)
        {
            var res = await _zoneFloorOperations.Update(zoneFloorDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneFloorDTO>>> Delete(Guid Id)
        {
            var res = await _zoneFloorOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpDelete("Foundation/{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> DeleteRoofAtticByIds(Guid Id, [FromKeyedServices("Foundation")] IFoundationOperations  foundationOperations)
        {
            var res = await foundationOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
