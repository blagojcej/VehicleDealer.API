using AutoMapper;
using VehicleDealer.API.Controllers.Resources;
using VehicleDealer.API.Models;

namespace VehicleDealer.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature, FeatureResource>();
        }
    }
}