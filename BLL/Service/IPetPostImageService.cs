using BLL.Dto;
using DAL.Model;
using Microsoft.AspNetCore.Http;

namespace BLL.Service;

public interface IPetPostImageService
{
    int AddPetPostImage(Post post, Pet pet, IFormFile image);

    IEnumerable<PetPostImageDto> FindAllPetPostImage(string sortOrder);

    PetPostImageDto? FindByPostId(int id);
}