

using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.Address;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations
{
    public interface IBuildingOperations : ICrudOperations<Building, BuildingDTO>
    {
        public Task<ResponseDTO<BuildingDTO>> CreateNewBuilding();
        public Task<ResponseDTO<BuildingDTO>> UpdateBuildingNumber(Guid Id,int? Number = null);
    }
    public class BuildingOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<Building, BuildingDTO>(_context, _context.Building,_mapper), IBuildingOperations
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
        public async override Task<ResponseDTO<BuildingDTO>> GetById(Guid Id)
        {
            var entities = await _context.Building
                .Include(obj=>obj.Address)
                .Include(obj=>obj.About)
                .Include(obj=>obj.ZoneFloor)
                    .ThenInclude(obj=>obj.Foundations)
                .Include(obj=>obj.ZoneRoof)
                    .ThenInclude(obj=>obj.RoofAttics)
                .Include(obj=>obj.ZoneWall)
                    .ThenInclude(obj=>obj.Walls)
                .Include(obj=>obj.ZoneWindow)
                    .ThenInclude(obj=>obj.Windows)
                .Include(obj=>obj.HeatingCoolingSystem)
                    .ThenInclude(obj=>obj.Systems)
                    .ThenInclude(obj=>obj.DuctLocations)
                .Include(obj=>obj.WaterHeater)
                .Include(obj=>obj.PVSystem)
                .FirstOrDefaultAsync(obj => obj.Id == Id);
            if (entities == null)
            {
                return new ResponseDTO<BuildingDTO> { Failed = true, Message = "Building not found" };
            }
            var entityDTO = _mapper.Map<BuildingDTO>(entities);
            return new ResponseDTO<BuildingDTO> { Failed = false, Message = "Building Fetched", Data = entityDTO };
        }
        public async Task<ResponseDTO<BuildingDTO>> UpdateBuildingNumber(Guid Id, int? Number = null)
        {
            var building = await _context.Building.FirstOrDefaultAsync(obj => obj.Id == Id);
            if (building == null)
            {
                return new ResponseDTO<BuildingDTO> { Failed = true, Message = "Building not found" };
            }
            building.Number = Number;
            var res = await base.Update(building);
            if (res.Failed)
            {
                return res;
            }
            return res;
        }
    }
}
