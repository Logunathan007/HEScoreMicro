using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity;
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

        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<PVSystemDTO>>> GetById(Guid Id)
        {
            var res = await _pVSystemOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<PVSystemDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _pVSystemOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<PVSystemDTO>>>> GetAll()
        {
            var res = await _pVSystemOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
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

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<PVSystemDTO>>> Delete(Guid Id)
        {
            var res = await _pVSystemOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}
