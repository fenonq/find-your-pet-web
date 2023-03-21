namespace BLL.Service.impl;

using DAL.Model;
using DAL.Repository;

public class ImageService : IImageService
{
    private readonly IEntityRepository<Image> _imageRepository;

    public ImageService(IEntityRepository<Image> imageRepository)
    {
        this._imageRepository = imageRepository;
    }

    public List<Image> FindAll()
    {
        return this._imageRepository.FindAll().ToList();
    }

    public Image? FindById(int id)
    {
        return this._imageRepository.FindById(id);
    }

    public void Add(string path, int petId)
    {
        var image = new Image()
        {
            Path = path,
            PetId = petId,
        };

        this._imageRepository.Add(image);
    }

    public void Remove(int id)
    {
        this._imageRepository.Remove(id);
    }
}