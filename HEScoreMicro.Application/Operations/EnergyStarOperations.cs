
using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.EnergyStars;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IEnergyStarOperations : ICrudOperations<EnergyStar, EnergyStarDTO>
    {
    }
    public class EnergyStarOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<EnergyStar, EnergyStarDTO>(_context, _context.EnergyStar, _mapper), IEnergyStarOperations
    {
    }
}
