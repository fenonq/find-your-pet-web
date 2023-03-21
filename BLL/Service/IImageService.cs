using DAL.Model;

namespace BLL.Service;

public interface IImageService
{
    List<Image> FindAll();

    Image? FindById(int id);

    void Add(string path, int petId);

    void Remove(int id);
}