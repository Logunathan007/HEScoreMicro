using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations.ZoneWindows;
using HEScoreMicro.Domain.Entity.ZoneWindows;
using HEScoreMicro.Domain.Entity.ZoneWindows;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneWindowController : ControllerBase
    {
        private readonly IZoneWindowOperations _zoneWindowOperations;
        public ZoneWindowController(IZoneWindowOperations zoneWindowOperations)
        {
            _zoneWindowOperations = zoneWindowOperations;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<ZoneWindowDTO>>> Post([FromBody] ZoneWindowDTO zoneWindowDTO)
        {
            var res = await _zoneWindowOperations.Add(zoneWindowDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<ZoneWindowDTO>>> Put([FromBody] ZoneWindowDTO zoneWindowDTO)
        {
            var res = await _zoneWindowOperations.Update(zoneWindowDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpDelete("[Action]")]
        public async Task<ActionResult<ResponseDTO<ZoneWindowDTO>>> DeleteByIds([FromBody] IEnumerable<Guid> Id, [FromKeyedServices("Window")] IWindowOperations windowOperations)
        {
            var res = await windowOperations.BulkDelete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
