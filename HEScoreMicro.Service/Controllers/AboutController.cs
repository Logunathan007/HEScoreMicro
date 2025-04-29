using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity.Address;
using Microsoft.AspNetCore.Mvc;

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutOperations _aboutOperations;
        public AboutController(IAboutOperations aboutOperations)
        {
            _aboutOperations = aboutOperations;
        }

/*        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<AboutDTO>>> GetById(Guid Id)
        {
            var res = await _aboutOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<AboutDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _aboutOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }

/*        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<AboutDTO>>>> GetAll()
        {
            var res = await _aboutOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<AboutDTO>>> Post([FromBody] AboutDTO aboutDTO)
        {
            var res = await _aboutOperations.Add(aboutDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<AboutDTO>>> Put([FromBody] AboutDTO aboutDTO)
        {
            var res = await _aboutOperations.Update(aboutDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

/*        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<AboutDTO>>> Delete(Guid Id)
        {
            var res = await _aboutOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
    }
}
