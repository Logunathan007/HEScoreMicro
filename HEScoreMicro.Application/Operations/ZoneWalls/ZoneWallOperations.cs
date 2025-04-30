
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Domain.Entity.ZoneWalls;
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
    }
}
