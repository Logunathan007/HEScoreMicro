using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations.ZoneWalls
{
    public interface IWallOperations : ICrudOperations<Wall, WallDTO>
    {
    }
    public class WallOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<Wall, WallDTO>(_context, _context.Wall, _mapper), IWallOperations
    {
    }
}
