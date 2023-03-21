using DAL.Model;
using DAL.Repository;

namespace BLL.Service.impl;

public class ImageService : IImageService
{
    private readonly IEntityRepository<Image> _imageRepository;

    public ImageService(IEntityRepository<Image> imageRepository)
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

    public void Add(string path, int petId)
    {
        var image = new Image()
        {
            Path = path,
            PetId = petId
        };

        _imageRepository.Add(image);
    }

    public void Remove(int id)
    {
        _imageRepository.Remove(id);
    }
}