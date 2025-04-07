
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.ZoneFloors
{
    public interface IZoneFloorOperations : ICrudOperations<ZoneFloor, ZoneFloorDTO>
    {
    }
    public class ZoneFloorOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<ZoneFloor, ZoneFloorDTO>(_context, _context.ZoneFloor, _mapper), IZoneFloorOperations
    {
        public override async Task<ResponseDTO<ZoneFloorDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _context.ZoneFloor.AsNoTracking().Include(x => x.Foundations)
                .FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<ZoneFloorDTO> { Failed = true, Message = $"ZoneFloor not found" };
            }
            var entityDTO = _mapper.Map<ZoneFloorDTO>(entities);
            return new ResponseDTO<ZoneFloorDTO> { Failed = false, Message = $"ZoneFloor Fetched", Data = entityDTO };
        }
    }
}
