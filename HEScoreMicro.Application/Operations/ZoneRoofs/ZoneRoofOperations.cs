
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.ZoneRoofs
{
    public interface IZoneRoofOperations : ICrudOperations<ZoneRoof, ZoneRoofDTO>
    {
    }
    public class ZoneRoofOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<ZoneRoof, ZoneRoofDTO>(_context, _context.ZoneRoof, _mapper), IZoneRoofOperations
    {
        public override async Task<ResponseDTO<ZoneRoofDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _context.ZoneRoof.AsNoTracking().Include(x => x.RoofAttics)
                .FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<ZoneRoofDTO> { Failed = true, Message = $"ZoneRoof not found" };
            }
            var entityDTO = _mapper.Map<ZoneRoofDTO>(entities);
            return new ResponseDTO<ZoneRoofDTO> { Failed = false, Message = $"ZoneRoof Fetched", Data = entityDTO };
        }
    }
}
