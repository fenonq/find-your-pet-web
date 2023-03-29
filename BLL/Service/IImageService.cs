namespace BLL.Service;

using DAL.Model;

public interface IImageService : ICrudService<Image>
{
    Image FindByPetId(int petId);
}