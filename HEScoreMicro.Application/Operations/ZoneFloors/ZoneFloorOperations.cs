
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.ZoneFloors;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations.ZoneFloors
{
    public interface IZoneFloorOperations : ICrudOperations<ZoneFloor, ZoneFloorDTO>
    {
    }
    public class ZoneFloorOperations(
        DbConnect _context, IMapper _mapper
        ) : CrudOperations<ZoneFloor, ZoneFloorDTO>(_context, _context.ZoneFloor, _mapper), IZoneFloorOperations
    {
    }
}
