using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity.OtherSystems;
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
    }
}
