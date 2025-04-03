
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IAboutOperations: ICrudOperations<About, AboutDTO>
    {
    }
    public class AboutOperations (
        DbConnect _context
        ) : CrudOperations<About, AboutDTO>(_context, _context.About), IAboutOperations
    {
        
    }
}
