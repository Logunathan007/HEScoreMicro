using HEScoreMicro.Application.Operations;

namespace HEScoreMicro.Service
{
    public static class SupportFile
    {
        public static void DependencyInjection(IServiceCollection service)
        {
            service.AddScoped(typeof(IAddressOperations), typeof(AddressOperations));
            service.AddScoped(typeof(IBuildingOperations), typeof(BuildingOperations));
            service.AddScoped(typeof(IAboutOperations), typeof(AboutOperations));
        }
    }
}