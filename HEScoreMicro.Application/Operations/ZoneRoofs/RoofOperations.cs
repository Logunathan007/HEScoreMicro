

using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations.ZoneRoofs
{
    public interface IRoofAtticOperations : ICrudOperations<RoofAttic, RoofAtticDTO>
    {
    }
    public class RoofAtticOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<RoofAttic, RoofAtticDTO>(_context, _context.RoofAttic, _mapper), IRoofAtticOperations
    {
    }
}
