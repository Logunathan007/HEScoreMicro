using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.ZoneFloors;
using HEScoreMicro.Domain.Entity.ZoneFloors;
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
