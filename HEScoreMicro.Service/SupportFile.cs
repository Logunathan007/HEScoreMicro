using HEScoreMicro.Application.Operations;
using HEScoreMicro.Application.Operations.HeatingCooling;
using HEScoreMicro.Application.Operations.HPXMLGeneration;
using HEScoreMicro.Application.Operations.ZoneFloors;
using HEScoreMicro.Application.Operations.ZoneRoofs;
using HEScoreMicro.Application.Operations.ZoneWalls;
using HEScoreMicro.Application.Operations.ZoneWindows;
using ServiceReference1;

namespace HEScoreMicro.Service
{
    public static class SupportFile
    {
        public static void DependencyInjection(IServiceCollection service)
        {
            service.AddScoped(typeof(IAddressOperations), typeof(AddressOperations));
            service.AddScoped(typeof(IBuildingOperations), typeof(BuildingOperations));
            service.AddScoped(typeof(IAboutOperations), typeof(AboutOperations));
            service.AddScoped(typeof(IZoneFloorOperations), typeof(ZoneFloorOperations));
            service.AddScoped(typeof(IZoneRoofOperations), typeof(ZoneRoofOperations));
            service.AddScoped(typeof(IZoneWallOperations), typeof(ZoneWallOperations));
            service.AddScoped(typeof(IWaterHeaterOperations), typeof(WaterHeaterOperations));
            service.AddScoped(typeof(IPVSystemOperations), typeof(PVSystemOperations));
            service.AddScoped(typeof(IZoneWindowOperations), typeof(ZoneWindowOperations));
            service.AddScoped(typeof(IHeatingCoolingSystemOperations), typeof(HeatingCoolingSystemOperations));
            service.AddScoped(typeof(IHPXMLGenerationOperations), typeof(HPXMLGenerationOperations));
            service.AddScoped(typeof(IHPXMLObjectCreation), typeof(HPXMLObjectCreation));
            service.AddScoped(typeof(IEnergyStarOperations), typeof(EnergyStarOperations));
            service.AddScoped(typeof(st_api_handlerPortClient), typeof(st_api_handlerPortClient));

            //Keyed Services
            service.AddKeyedScoped<IWindowOperations, WindowOperations>("Window");
            service.AddKeyedScoped<IWallOperations, WallOperations>("Wall");
            service.AddKeyedScoped<IDuctLocationOperations, DuctLocationOperations>("DuctLocation");
            service.AddKeyedScoped<ISystemsOperations, SystemsOperations>("Systems");
            service.AddKeyedScoped<IRoofAtticOperations, RoofAtticOperations>("RoofAttic");
            service.AddKeyedScoped<IFoundationOperations, FoundationOperations>("Foundation");
        }
    }
}