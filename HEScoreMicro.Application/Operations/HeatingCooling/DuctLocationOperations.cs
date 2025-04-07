

using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations.HeatingCooling
{
    public interface IDuctLocationOperations : ICrudOperations<DuctLocation, DuctLocationDTO>
    {
    }
    public class DuctLocationOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<DuctLocation, DuctLocationDTO>(_context, _context.DuctLocation, _mapper), IDuctLocationOperations
    {
    }
}
