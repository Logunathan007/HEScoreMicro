using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.OtherSystems;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IPVSystemOperations : ICrudOperations<PVSystem, PVSystemDTO>
    {
    }
    public class PVSystemOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<PVSystem, PVSystemDTO>(_context, _context.PVSystem, _mapper), IPVSystemOperations
    {
    }
}
