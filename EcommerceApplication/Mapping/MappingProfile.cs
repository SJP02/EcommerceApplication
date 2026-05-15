using AutoMapper;
using EcommerceApplication.Models;
using EcommerceApplication.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product → ProductDTO mapping
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CompanyName,
                       opt => opt.MapFrom(src => src.ProductCompany != null ? src.ProductCompany.CompanyName : string.Empty))
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.ProductCategory != null ? src.ProductCategory.CategoryName : string.Empty));

        // Company → CompanyDTO mapping
        CreateMap<Company, CompanyDTO>()
             .ForMember(dest => dest.ProductList,
                       opt => opt.MapFrom(src => src.ProductList)); 

        // ProductPatchDTO → Product
        CreateMap<ProductPatchDTO, Product>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));

        // CompanyPatchDTO → Company
        CreateMap<CompanyPatchDTO, Company>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}