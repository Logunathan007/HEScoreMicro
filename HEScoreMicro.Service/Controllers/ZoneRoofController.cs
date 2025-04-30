using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.ZoneRoofs;
using HEScoreMicro.Application.Operations.ZoneWalls;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneRoofController : ControllerBase
    {
        private readonly IZoneRoofOperations _zoneRoofOperations;
        public ZoneRoofController(IZoneRoofOperations zoneRoofOperations)
        {
            _zoneRoofOperations = zoneRoofOperations;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<ZoneRoofDTO>>> Post([FromBody] ZoneRoofDTO zoneRoofDTO)
        {
            var res = await _zoneRoofOperations.Add(zoneRoofDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<ZoneRoofDTO>>> Put([FromBody] ZoneRoofDTO zoneRoofDTO)
        {
            var res = await _zoneRoofOperations.Update(zoneRoofDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpDelete("RoofAttic/{id}")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> DeleteRoofAtticByIds(Guid Id, [FromKeyedServices("RoofAttic")] IRoofAtticOperations roofAtticOperations)
        {
            var res = await roofAtticOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
