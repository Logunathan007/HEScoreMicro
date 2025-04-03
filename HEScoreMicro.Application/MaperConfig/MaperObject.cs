using AutoMapper;

namespace HEScoreMicro.Application.MaperConfig
{
    public class MaperObject<TSource, TDestination>
    {
        private readonly IMapper mapper;
        public MaperObject()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>().ReverseMap();
            });
            this.mapper = config.CreateMapper();
        }
        protected virtual TDestination ResponseMap(TSource source)
        {
            return mapper.Map<TDestination>(source);
        }
        protected virtual ICollection<TDestination> ResponseMap(ICollection<TSource> source)
        {
            return mapper.Map<ICollection<TDestination>>(source);
        }
        protected virtual TSource RequestMap(TDestination destination)
        {
            return mapper.Map<TSource>(destination);
        }
        protected virtual ICollection<TSource> RequestMap(ICollection<TDestination> destination)
        {
            return mapper.Map<ICollection<TSource>>(destination);
        }
    }
}
