
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity.Address;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.Operations
{
    public interface IAddressOperations : ICrudOperations<Address, AddressDTO>
    {
        public Task<ResponseDTO<AddressDTO>> AddAddress(AddressDTO entityDTO);
        public Task<ResponseDTO<AddressDTO>> GetByBuidlgingId(Guid Id);
    }
    public class AddressOperations(
        DbConnect _context, IBuildingOperations _buildingOperations, IMapper _mapper
        ) : CrudOperations<Address, AddressDTO>(_context, _context.Address,_mapper), IAddressOperations
    {
        public async Task<ResponseDTO<AddressDTO>> AddAddress(AddressDTO entityDTO)
        {
            var buildingRes = await _buildingOperations.CreateNewBuilding();
            if (buildingRes.Failed)
            {
                return new ResponseDTO<AddressDTO> { Failed = true, Message = buildingRes.Message };
            }
            entityDTO.BuildingId = buildingRes.Data.Id;
            var addressRes = await base.Add(entityDTO);
            if (addressRes.Failed)
            {
                return new ResponseDTO<AddressDTO> { Failed = true, Message = addressRes.Message };
            }
            return addressRes;
        }
        public async Task<ResponseDTO<AddressDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _context.Address.AsNoTracking().FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<AddressDTO> { Failed = true, Message = $"Address not found" };
            }
            var entityDTO = _mapper.Map<AddressDTO>(entities);
            return new ResponseDTO<AddressDTO> { Failed = false, Message = $"Address Fetched", Data = entityDTO };
        }
    }
}
