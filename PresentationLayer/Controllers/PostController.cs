using AutoMapper;
using BLL.Service;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class PostController : Controller
{
    private readonly IMapper _mapper;
    private readonly IPostService _postService;
    private readonly ILogger<AccountController> _logger;
    private readonly IPetPostImageService _petPostImageService;

    public PostController(
        IMapper mapper,
        IPostService postService,
        ILogger<AccountController> logger,
        IPetPostImageService petPostImageService)
    {
        _logger = logger;
        _mapper = mapper;
        _postService = postService;
        _petPostImageService = petPostImageService;
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        _logger.LogInformation("Show CreatePost form");
        return View();
    }

    [HttpPost]
    public IActionResult CreatePost(PetPostImageViewModel model)
    {
        _logger.LogInformation("Creating post..");
        var pet = _mapper.Map<Pet>(model.Pet);
        var post = _mapper.Map<Post>(model.Post);
        _petPostImageService.AddPetPostImage(post, pet, model.Post.Photo);
        _logger.LogInformation("Post successfully created");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AllPosts(string sortOrder)
    {
        _logger.LogInformation("Show AllPosts..");
        var petPosts = _mapper.Map<IEnumerable<PetPostImageViewModel>>(_petPostImageService
            .FindAllPetPostImage(sortOrder));
        return View(petPosts);
    }
    [HttpGet]
    public IActionResult MyPosts(string sortOrder)
    {
        _logger.LogInformation("Show AllPosts..");
        var petPosts = _mapper.Map<IEnumerable<PetPostImageViewModel>>(_petPostImageService
            .FindAllPetPostImageByUser(sortOrder, 2));
        return View("AllPosts", petPosts);
    }
    [HttpGet]
    public IActionResult PostDetails(int id)
    {
        _logger.LogInformation("Show PostDetails..");
        try
        {
            var postExtensionModel =
                _mapper.Map<PetPostImageViewModel>(_petPostImageService.FindByPostId(id));
            return View(postExtensionModel);
        }
        catch (Exception)
        {
            return View("Error");
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