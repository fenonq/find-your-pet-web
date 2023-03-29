using AutoMapper;
using BLL.Service;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class PostController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IPostService _postService;
    private readonly IPetService _petService;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PostController(
        ILogger<AccountController> logger,
        IPostService postService,
        IMapper mapper,
        IPetService petService,
        IWebHostEnvironment webHostEnvironment,
        IImageService imageService)
    {
        _logger = logger;
        _postService = postService;
        _mapper = mapper;
        _petService = petService;
        _webHostEnvironment = webHostEnvironment;
        _imageService = imageService;
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        _logger.LogInformation("Show CreatePost form");
        return View();
    }

    [HttpPost]
    public IActionResult CreatePost(PetPostViewModel model)
    {
        _logger.LogInformation("Creating post..");
        var pet = _mapper.Map<Pet>(model.Pet);

        pet.OwnerId = 1;
        _petService.Add(pet);

        var post = _mapper.Map<Post>(model.Post);
        post.PetId = pet.Id;
        post.UserId = 1;

        if (model.Post.Photo.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(model.Post.Photo.FileName);
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                model.Post.Photo.CopyTo(stream);
            }

            _imageService.Add(new Image
            {
                Path = "/uploads/" + fileName,
                PetId = pet.Id,
            });
        }

        _postService.Add(post);
        _logger.LogInformation("Post successfully created");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AllPosts(string sortOrder)
    {
        _logger.LogInformation("Show AllPosts..");
        ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date" : string.Empty;

        var findAllPosts = _postService.FindAll().Where(p => p.IsActive);
        var findAllPets = _petService.FindAll();
        var findAllImages = _imageService.FindAll();

        var postsWithPets = from post in findAllPosts
            join pet in findAllPets on post.PetId equals pet.Id
            join image in findAllImages on pet.Id equals image.PetId
            select new PetPostViewModel
            {
                Post = _mapper.Map<PostViewModel>(post),
                Pet = _mapper.Map<PetViewModel>(pet),
                Image = _mapper.Map<ImageViewModel>(image),
            };

        IEnumerable<PetPostViewModel> petPosts = sortOrder switch
        {
            "lost_date" => postsWithPets.OrderByDescending(pp => pp.Post.Date),
            "location" => postsWithPets.OrderByDescending(pp => pp.Post.Location),
            "type" => postsWithPets.OrderByDescending(pp => pp.Post.Type),
            _ => postsWithPets.OrderBy(pp => pp.Post.Date)
        };

        return View(petPosts);
    }

    [HttpGet]
    public IActionResult PostDetails(int id)
    {
        try
        {
            _logger.LogInformation("Show PostDetails..");

            var post = _postService.FindById(id);
            var pet = _petService.FindById(post.PetId);
            var image = _imageService.FindByPetId(post.PetId);

            var postExtensionModel = new PetPostViewModel
            {
                Post = _mapper.Map<PostViewModel>(post),
                Pet = _mapper.Map<PetViewModel>(pet),
                Image = _mapper.Map<ImageViewModel>(image),
            };
            return View(postExtensionModel);
        }
        catch (Exception)
        {
            return View("Error");
            throw;
        }
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Deleting post..");
        _postService.ChangeVisibility(id);
        _logger.LogInformation("Post successfully deleted");
        return RedirectToAction(nameof(AllPosts));
    }
}