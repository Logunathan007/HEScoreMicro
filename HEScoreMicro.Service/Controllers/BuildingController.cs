using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Operations;
using HEScoreMicro.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HEScoreMicro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingOperations _buildingOperations;
        public BuildingController(IBuildingOperations buildingOperations)
        {
            _buildingOperations = buildingOperations;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDTO<BuildingDTO>>> GetById(Guid Id)
        {
            var res = await _buildingOperations.GetById(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseDTO<ICollection<BuildingDTO>>>> GetAll()
        {
            var res = await _buildingOperations.GetAll();
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDTO<BuildingDTO>>> Post([FromBody] BuildingDTO buildingDTO)
        {
            var res = await _buildingOperations.Add(buildingDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<BuildingDTO>>> Put([FromBody] BuildingDTO buildingDTO)
        {
            var res = await _buildingOperations.Update(buildingDTO);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ResponseDTO<BuildingDTO>>> Delete(Guid Id)
        {
            var res = await _buildingOperations.Delete(Id);
            if (res.Failed)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
    }
}