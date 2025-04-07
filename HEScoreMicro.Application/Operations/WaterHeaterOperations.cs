using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IWaterHeaterOperations : ICrudOperations<WaterHeater, WaterHeaterDTO>
    {
    }
    public class WaterHeaterOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<WaterHeater, WaterHeaterDTO>(_context, _context.WaterHeater, _mapper), IWaterHeaterOperations
    {
    }
}
