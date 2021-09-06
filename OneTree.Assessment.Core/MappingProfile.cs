using AutoMapper;
using OneTree.Assessment.Core.Dtos;

namespace OneTree.Assessment.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, Domain.Entities.Product>().ReverseMap();
            CreateMap<ProductToUpdate, Domain.Entities.Product>().ReverseMap();
            CreateMap<ProductToCreate, Product>().ReverseMap();
            CreateMap<ProductToUpdate, Product>().ReverseMap();
        }
    }
}
