
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneWindow;
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
        public override async Task<ResponseDTO<ZoneWindowDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _context.ZoneWindow.AsNoTracking().Include(x => x.Windows)
                .FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<ZoneWindowDTO> { Failed = true, Message = $"ZoneWindow not found" };
            }
            var entityDTO = _mapper.Map<ZoneWindowDTO>(entities);
            return new ResponseDTO<ZoneWindowDTO> { Failed = false, Message = $"ZoneWindow Fetched", Data = entityDTO };
        }
    }
}
