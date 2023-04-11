using BLL.Dto;
using DAL.Model;
using Microsoft.AspNetCore.Http;

namespace BLL.Service;

public interface IPetPostImageService
{
    int AddPetPostImage(Post post, Pet pet, int userId, IFormFile image);

    IEnumerable<PetPostImageDto> FindAllPetPostImage(string sortOrder);

    IEnumerable<PetPostImageDto> FindAllPetPostImageByUser(string sortOrder, int userId);

    PetPostImageDto? FindByPostId(int id);
}