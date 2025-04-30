
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneWindows;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.ZoneWindows
{
    public interface IZoneWindowOperations : ICrudOperations<ZoneWindow, ZoneWindowDTO>
    {
    }
    public class ZoneWindowOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<ZoneWindow, ZoneWindowDTO>(_context, _context.ZoneWindow, _mapper), IZoneWindowOperations
    {
    }
}
