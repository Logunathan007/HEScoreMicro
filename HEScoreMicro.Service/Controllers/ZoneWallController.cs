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

/*        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> GetById(Guid Id)
        {
            var res = await _zoneWallOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _zoneWallOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

 /*       [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<ZoneWallDTO>>>> GetAll()
        {
            var res = await _zoneWallOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/

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

/*        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<ZoneWallDTO>>> Delete(Guid Id)
        {
            var res = await _zoneWallOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
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
