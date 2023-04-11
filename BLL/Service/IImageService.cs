using DAL.Model;
using Microsoft.AspNetCore.Http;

namespace BLL.Service;

public interface IImageService : ICrudService<Image>
{
    Image FindByPetId(int petId);

    Task<bool> UploadUserPhoto(User user, IFormFile file);

    string GetUserImage(User user);
}