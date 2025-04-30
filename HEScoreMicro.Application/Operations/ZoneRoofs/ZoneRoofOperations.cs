
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
    }
}
