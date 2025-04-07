
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
        public override async Task<ResponseDTO<HeatingCoolingSystemDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _context.HeatingCoolingSystem.AsNoTracking()
                .Include(x => x.Systems).ThenInclude(obj => obj.DuctLocations)
                .FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<HeatingCoolingSystemDTO> { Failed = true, Message = $"HeatingCoolingSystem not found" };
            }
            var entityDTO = _mapper.Map<HeatingCoolingSystemDTO>(entities);
            return new ResponseDTO<HeatingCoolingSystemDTO> { Failed = false, Message = $"HeatingCoolingSystem Fetched", Data = entityDTO };
        }
    }
}
