
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Domain.Entity.ZoneWall;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.ZoneWalls
{
    public interface IZoneWallOperations : ICrudOperations<ZoneWall, ZoneWallDTO>
    {
    }
    public class ZoneWallOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<ZoneWall, ZoneWallDTO>(_context, _context.ZoneWall, _mapper), IZoneWallOperations
    {
        public override async Task<ResponseDTO<ZoneWallDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _context.ZoneWall.AsNoTracking().Include(x => x.Walls)
                .FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<ZoneWallDTO> { Failed = true, Message = $"ZoneWall not found" };
            }
            var entityDTO = _mapper.Map<ZoneWallDTO>(entities);
            return new ResponseDTO<ZoneWallDTO> { Failed = false, Message = $"ZoneWall Fetched", Data = entityDTO };
        }
    }
}
