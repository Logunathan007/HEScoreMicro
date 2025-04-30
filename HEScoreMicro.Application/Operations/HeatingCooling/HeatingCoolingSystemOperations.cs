
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.HeatingCooling
{
    public interface IHeatingCoolingSystemOperations : ICrudOperations<HeatingCoolingSystem, HeatingCoolingSystemDTO>
    {
    }
    public class HeatingCoolingSystemOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<HeatingCoolingSystem, HeatingCoolingSystemDTO>(_context, _context.HeatingCoolingSystem, _mapper), IHeatingCoolingSystemOperations
    {
    }
}
