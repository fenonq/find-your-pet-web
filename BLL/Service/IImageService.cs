using DAL.Model;

namespace BLL.Service;

public interface IImageService : ICrudService<Image>
{
    Image FindByPetId(int petId);
}