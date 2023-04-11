using DAL.Model;
using DAL.Repository;

namespace BLL.Service.impl;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public List<Image> FindAll()
    {
        return _imageRepository.FindAll().ToList();
    }

    public Image? FindById(int id)
    {
        return _imageRepository.FindById(id);
    }

    public Image FindByPetId(int petId)
    {
        return _imageRepository.FindAll().FirstOrDefault(p => p.PetId == petId);
    }

    public int Add(Image image)
    {
        _imageRepository.Add(image);
        return image.Id;
    }

    public void Remove(int id)
    {
        _imageRepository.Remove(id);
    }
}