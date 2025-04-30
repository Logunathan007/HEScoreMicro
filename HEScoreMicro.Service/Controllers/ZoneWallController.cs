using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.ZoneWalls;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneWallController : ControllerBase
    {
        private readonly IZoneWallOperations _zoneWallOperations;
        public ZoneWallController(IZoneWallOperations zoneWallOperations)
        {
            _zoneWallOperations = zoneWallOperations;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> Post([FromBody] ZoneWallDTO zoneWallDTO)
        {
            var res = await _zoneWallOperations.Add(zoneWallDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> Put([FromBody] ZoneWallDTO zoneWallDTO)
        {
            var res = await _zoneWallOperations.Update(zoneWallDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpDelete("[Action]")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> DeleteByIds([FromBody] IEnumerable<Guid> Id, [FromKeyedServices("Wall")] IWallOperations wallOperations )
        {
            var res = await wallOperations.BulkDelete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
