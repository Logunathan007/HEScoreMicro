using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterHeaterController : ControllerBase
    {
        private readonly IWaterHeaterOperations _waterHeaterOperations;
        public WaterHeaterController(IWaterHeaterOperations waterHeaterOperations)
        {
            _waterHeaterOperations = waterHeaterOperations;
        }

/*        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<WaterHeaterDTO>>> GetById(Guid Id)
        {
            var res = await _waterHeaterOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<WaterHeaterDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _waterHeaterOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

/*        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<WaterHeaterDTO>>>> GetAll()
        {
            var res = await _waterHeaterOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<WaterHeaterDTO>>> Post([FromBody] WaterHeaterDTO waterHeaterDTO)
        {
            var res = await _waterHeaterOperations.Add(waterHeaterDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<WaterHeaterDTO>>> Put([FromBody] WaterHeaterDTO waterHeaterDTO)
        {
            var res = await _waterHeaterOperations.Update(waterHeaterDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

/*        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<WaterHeaterDTO>>> Delete(Guid Id)
        {
            var res = await _waterHeaterOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
    }
}
