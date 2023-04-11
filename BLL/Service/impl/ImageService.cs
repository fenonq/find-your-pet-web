using DAL.Model;
using DAL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Service.impl;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageService(IImageRepository imageRepository, IWebHostEnvironment webHostEnvironment)
    {
        _imageRepository = imageRepository;
        _webHostEnvironment = webHostEnvironment;
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

    public async Task<bool> UploadUserPhoto(User user, IFormFile file)
    {
        if (file.Length > 0)
        {
            var fileName = user.Id + ".png";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", fileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }

        return false;
    }

    public string GetUserImage(User user)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", user.Id + ".png");

        return System.IO.File.Exists(path) ? "/userPhotos/" + user.Id + ".jpg" :
                                             "/userPhotos/" + "default.png";
    }
}