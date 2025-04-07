using AutoMapper;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneWindow;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations.ZoneWindows
{
    public interface IWindowOperations : ICrudOperations<Window, WindowDTO>
    {
    }
    public class WindowOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<Window, WindowDTO>(_context, _context.Window, _mapper), IWindowOperations
    {
    }
}
