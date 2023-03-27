namespace BLL.Service.impl;

using DAL.Model;
using DAL.Repository;

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