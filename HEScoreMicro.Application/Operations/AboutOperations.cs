
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.Address;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IAboutOperations: ICrudOperations<About, AboutDTO>
    {
    }
    public class AboutOperations (
        DbConnect _context,IMapper _mapper
        ) : CrudOperations<About, AboutDTO>(_context, _context.About, _mapper), IAboutOperations
    {
        
    }
}
