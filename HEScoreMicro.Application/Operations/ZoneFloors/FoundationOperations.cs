using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneFloors;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.ZoneFloors
{
    public interface IFoundationOperations : ICrudOperations<Foundation, FoundationDTO>
    {
    }
    public class FoundationOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<Foundation, FoundationDTO>(_context, _context.Foundation, _mapper), IFoundationOperations
    {
    }
}
