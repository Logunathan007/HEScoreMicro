using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Application.Reply;
using HEScoreMicro.Domain.Entity.Address;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressOperations _addressOperations;
        public AddressController(IAddressOperations addressOperations)
        {
            _addressOperations = addressOperations;
        }

/*        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<AddressDTO>>> GetById(Guid Id)
        {
            var res = await _addressOperations.GetById(Id);
            if (res.Failed)
            {
                return StatusCode(204);
            }
            return Ok(res);
        }*/
        [HttpGet("[action]/{Id}")]
        public async Task<ActionResult<ResponseDTO<AddressDTO>>> GetByBuildingId(Guid Id)
        {
            var res = await _addressOperations.GetByBuidlgingId(Id);
            if (res.Failed)
            {
                return NoContent();
            }
            return Ok(res);
        }

/*        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<AddressDTO>>>> GetAll()
        {
            var res = await _addressOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<AddressDTO>>> Post([FromBody] AddressDTO addressDTO)
        {
            var res = await _addressOperations.AddAddress(addressDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<AddressDTO>>> Put([FromBody] AddressDTO addressDTO)
        {
            var res = await _addressOperations.Update(addressDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

/*        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<AddressDTO>>> Delete(Guid Id)
        {
            var res = await _addressOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }*/
    }
}