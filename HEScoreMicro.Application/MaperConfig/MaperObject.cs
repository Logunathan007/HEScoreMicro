using AutoMapper;
using HEScoreMicro.Domain.Entity.Address;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using HEScoreMicro.Domain.Entity.OtherSystems;
using HEScoreMicro.Domain.Entity.ZoneFloors;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using HEScoreMicro.Domain.Entity.ZoneWindows;

namespace HEScoreMicro.Application.MaperConfig
{
    public class MaperObject : Profile
    {
        public MaperObject()
        {
            CreateMap<Foundation, FoundationDTO>().ReverseMap();
            CreateMap<ZoneFloor, ZoneFloorDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Building, BuildingDTO>().ReverseMap();
            CreateMap<About, AboutDTO>().ReverseMap();
            CreateMap<RoofAttic, RoofAtticDTO>().ReverseMap();
            CreateMap<ZoneRoof, ZoneRoofDTO>().ReverseMap();
            CreateMap<ZoneWall, ZoneWallDTO>().ReverseMap();
            CreateMap<Wall, WallDTO>().ReverseMap();
            CreateMap<WaterHeater,WaterHeaterDTO>().ReverseMap();
            CreateMap<PVSystem, PVSystemDTO>().ReverseMap();
            CreateMap<Window, WindowDTO>().ReverseMap();
            CreateMap<ZoneWindow, ZoneWindowDTO>().ReverseMap();    
            CreateMap<DuctLocation, DuctLocationDTO>().ReverseMap();
            CreateMap<Systems, SystemsDTO>().ReverseMap();
            CreateMap<HeatingCoolingSystem, HeatingCoolingSystemDTO>().ReverseMap();
        }
    }
}
