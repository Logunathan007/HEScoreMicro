
using AutoMapper;
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.MaperConfig;
using HEScoreMicro.Domain.Entity;
using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;

namespace HEScoreMicro.Application.CrudOperations
{
    public interface ICrudOperations<TEntity, TEntityDTO>
    {
        public Task<ResponseDTO<TEntityDTO>> GetById(Guid Id);
        public Task<ResponseDTO<TEntityDTO>> GetByBuidlgingId(Guid Id);
        public Task<ResponseDTO<TEntityDTO>> Add(TEntity entity);
        public Task<ResponseDTO<TEntityDTO>> Add(TEntityDTO entityDTO);
        public Task<ResponseDTO<ICollection<TEntityDTO>>> GetAll();
        public Task<ResponseDTO<TEntityDTO>> Update(TEntity entity);
        public Task<ResponseDTO<TEntityDTO>> Update(TEntityDTO entityDTO);
        public Task<ResponseDTO<TEntityDTO>> Delete(Guid Id);
        public Task<ResponseDTO<TEntityDTO>> BulkDelete(IEnumerable<Guid> ids);
    }
    public class CrudOperations<TEntity, TEntityDTO> : ICrudOperations<TEntity, TEntityDTO> where TEntity : class, IHasId, IHasBuildingId
    {
        private DbSet<TEntity> _table { get; set; }
        private string TableName { get; set; }
        private DbConnect _context { get; set; }
        private IMapper _mapper { get; set; }
        public CrudOperations(DbConnect context, DbSet<TEntity> table, IMapper mapper)
        {
            _context = context;
            _table = table;
            TableName = typeof(TEntity).Name;
            _mapper = mapper;
        }
        public async Task<ResponseDTO<TEntityDTO>> GetById(Guid Id)
        {
            var entities = await _table.AsNoTracking().FirstOrDefaultAsync(obj => obj.Id == Id);
            if (entities == null)
            {
                return new ResponseDTO<TEntityDTO> { Failed = true, Message = $"{TableName} not found" };
            }
            var entityDTO = _mapper.Map<TEntityDTO>(entities);
            return new ResponseDTO<TEntityDTO> { Failed = false, Message = $"{TableName} Fetched", Data = entityDTO };
        }
        public virtual async Task<ResponseDTO<TEntityDTO>> GetByBuidlgingId(Guid Id)
        {
            var entities = await _table.AsNoTracking().FirstOrDefaultAsync(obj => obj.BuildingId == Id);
            if (entities == null)
            {
                return new ResponseDTO<TEntityDTO> { Failed = true, Message = $"{TableName} not found" };
            }
            var entityDTO = _mapper.Map<TEntityDTO>(entities);
            return new ResponseDTO<TEntityDTO> { Failed = false, Message = $"{TableName} Fetched", Data = entityDTO };
        }
        public async Task<ResponseDTO<ICollection<TEntityDTO>>> GetAll()
        {
            var entities = await _table.ToListAsync();
            if (entities == null)
            {
                return new ResponseDTO<ICollection<TEntityDTO>> { Failed = true, Message = $"{TableName} not found" };
            }
            var entityDTO = _mapper.Map<ICollection<TEntityDTO>>(entities);
            return new ResponseDTO<ICollection<TEntityDTO>> { Failed = false, Message = $"{TableName} Fetched", Data = entityDTO };
        }
        public async Task<ResponseDTO<TEntityDTO>> Add(TEntity entity)
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync(); // Save changes to the database
            var entityDTO = _mapper.Map<TEntityDTO>(entity);
            return new ResponseDTO<TEntityDTO> { Failed = false, Message = $"{TableName} Added", Data = entityDTO };
        }
        public virtual async Task<ResponseDTO<TEntityDTO>> Add(TEntityDTO entityDTO)
        {
            TEntity entity = _mapper.Map<TEntity>(entityDTO);
            return await Add(entity);
        }
        public async Task<ResponseDTO<TEntityDTO>> Update(TEntity entity)
        {
            _table.Update(entity);
            await _context.SaveChangesAsync(); // Save changes to the database
            var entityDTO = _mapper.Map<TEntityDTO>(entity);
            return new ResponseDTO<TEntityDTO> { Failed = false, Message = $"{TableName} Updated", Data=entityDTO };
        }
        public async Task<ResponseDTO<TEntityDTO>> Update(TEntityDTO entityDTO)
        {
            var entity = _mapper.Map<TEntity>(entityDTO);
            return await Update(entity);
        }
        public async Task<ResponseDTO<TEntityDTO>> Delete(Guid Id)
        {
            var entity = _table.Where(obj => obj.Id == Id);
            if (entity == null)
            {
                return new ResponseDTO<TEntityDTO> { Failed = true, Message = $"{TableName} not found" };
            }
            await entity.ExecuteDeleteAsync();
            await _context.SaveChangesAsync(); // Save changes to the database
            return new ResponseDTO<TEntityDTO> { Failed = false, Message = $"{TableName} Deleted" };
        }
        public async Task<ResponseDTO<TEntityDTO>> BulkDelete(IEnumerable<Guid> ids)
        {
            int count = await _table.Where(obj => ids.Contains(obj.Id)).ExecuteDeleteAsync();
            return new ResponseDTO<TEntityDTO> { Failed = false, Message = $"{count} {TableName} Deleted" };
        }
    }
}
