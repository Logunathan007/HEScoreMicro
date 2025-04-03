
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.CrudOperations;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Persistence.MakeConnection;

namespace HEScoreMicro.Application.Operations
{
    public interface IAddressOperations : ICrudOperations<Address, AddressDTO>
    {
        public Task<ResponseDTO<AddressDTO>> AddAddress(AddressDTO entityDTO);
    }
    public class AddressOperations(
        DbConnect _context, IBuildingOperations _buildingOperations
        ) : CrudOperations<Address, AddressDTO>(_context, _context.Address), IAddressOperations
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
    }
}
