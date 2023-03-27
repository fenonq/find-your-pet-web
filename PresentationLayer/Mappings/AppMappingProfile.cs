using AutoMapper;
using DAL.Model;
using PresentationLayer.Models;

namespace PresentationLayer.Mappings;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegisterViewModel, User>();
        CreateMap<PostViewModel, Post>().ReverseMap();
        CreateMap<PetViewModel, Pet>().ReverseMap();
        CreateMap<ImageViewModel, Image>().ReverseMap();
    }
}