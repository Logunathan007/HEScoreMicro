

using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IBuildingOperations : ICrudOperations<Building, BuildingDTO>
    {
        public Task<ResponseDTO<BuildingDTO>> CreateNewBuilding();
    }
    public class BuildingOperations(
        DbConnect _context
        ) : CrudOperations<Building, BuildingDTO>(_context, _context.Building), IBuildingOperations
    {
        public async Task<ResponseDTO<BuildingDTO>> CreateNewBuilding()
        {
            Building building= new Building() ;
            var res = await base.Add(building);
            if(res.Failed)
            {
                return res;
            }
            return res;
        }
    }
}
