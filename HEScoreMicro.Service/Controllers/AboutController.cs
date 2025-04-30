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
    }
}
