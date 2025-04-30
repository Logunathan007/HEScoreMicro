using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity.OtherSystems;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PVSystemController : ControllerBase
    {
        private readonly IPVSystemOperations _pVSystemOperations;
        public PVSystemController(IPVSystemOperations pVSystemOperations)
        {
            _pVSystemOperations = pVSystemOperations;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<PVSystemDTO>>> Post([FromBody] PVSystemDTO pVSystemDTO)
        {
            var res = await _pVSystemOperations.Add(pVSystemDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<PVSystemDTO>>> Put([FromBody] PVSystemDTO pVSystemDTO)
        {
            var res = await _pVSystemOperations.Update(pVSystemDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
