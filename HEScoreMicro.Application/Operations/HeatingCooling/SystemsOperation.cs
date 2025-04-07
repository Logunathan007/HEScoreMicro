
using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations.HeatingCooling
{
    public interface ISystemsOperations : ICrudOperations<Systems, SystemsDTO>
    {
    }
    public class SystemsOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<Systems, SystemsDTO>(_context, _context.Systems, _mapper), ISystemsOperations
    {
    }
}
