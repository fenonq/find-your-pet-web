using BLL.Dto;
using DAL.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Service.impl;

public class PetPostImageService : IPetPostImageService
{
    private readonly IPostService _postService;
    private readonly IPetService _petService;
    private readonly IImageService _imageService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PetPostImageService(
        IPostService postService,
        IPetService petService,
        IImageService imageService,
        IWebHostEnvironment webHostEnvironment)
    {
        _postService = postService;
        _petService = petService;
        _imageService = imageService;
        _webHostEnvironment = webHostEnvironment;
    }

    public int AddPetPostImage(Post post, Pet pet, IFormFile image)
    {
        post.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        post.IsActive = true;

        pet.OwnerId = 1;
        _petService.Add(pet);

        post.PetId = pet.Id;
        post.UserId = 1;
        _postService.Add(post);

        if (image.Length <= 0)
        {
            return post.Id;
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        _imageService.Add(new Image
        {
            Path = "/uploads/" + fileName,
            PetId = pet.Id,
        });

        return post.Id;
    }

    public IEnumerable<PetPostImageDto> FindAllPetPostImage(string sortOrder)
    {
        var findAllPosts = _postService.FindAll().Where(p => p.IsActive);
        var findAllPets = _petService.FindAll();
        var findAllImages = _imageService.FindAll();

        var postsWithPets = from post in findAllPosts
            join pet in findAllPets on post.PetId equals pet.Id
            join image in findAllImages on pet.Id equals image.PetId
            select new PetPostImageDto
            {
                Post = post,
                Pet = pet,
                Image = image,
            };

        IEnumerable<PetPostImageDto> petPosts = sortOrder switch
        {
            "lost_date" => postsWithPets.OrderByDescending(pp => pp.Post.Date),
            "location" => postsWithPets.OrderByDescending(pp => pp.Post.Location),
            "type" => postsWithPets.OrderByDescending(pp => pp.Post.Type),
            _ => postsWithPets.OrderBy(pp => pp.Post.Date)
        };

        return petPosts;
    }

    public PetPostImageDto? FindByPostId(int id)
    {
        var post = _postService.FindById(id);
        var pet = _petService.FindById(post.PetId);
        var image = _imageService.FindByPetId(post.PetId);

        var petPostImageDto = new PetPostImageDto
        {
            Post = post,
            Pet = pet,
            Image = image,
        };

        return petPostImageDto;
    }
}