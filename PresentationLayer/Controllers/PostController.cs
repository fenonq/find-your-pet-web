using AutoMapper;
using BLL.Service;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[Authorize]
public class PostController : Controller
{
    private readonly IMapper _mapper;
    private readonly IPostService _postService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IPetPostImageService _petPostImageService;

    public PostController(
        IMapper mapper,
        IPostService postService,
        ILogger<AccountController> logger,
        IPetPostImageService petPostImageService,
        UserManager<User> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _postService = postService;
        _petPostImageService = petPostImageService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        _logger.LogInformation("Show CreatePost form");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(PetPostImageViewModel model)
    {
        _logger.LogInformation("Creating post..");
        var pet = _mapper.Map<Pet>(model.Pet);
        var post = _mapper.Map<Post>(model.Post);
        var user = await _userManager.GetUserAsync(User);
        _petPostImageService.AddPetPostImage(post, pet, user.Id, model.Post.Photo);
        _logger.LogInformation("Post successfully created");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    //[AllowAnonymous]
    [Authorize(Roles = "admin")] // to test roles

    public IActionResult AllPosts(string sortOrder)
    {
        _logger.LogInformation("Show AllPosts..");
        var petPosts = _mapper.Map<IEnumerable<PetPostImageViewModel>>(_petPostImageService
            .FindAllPetPostImage(sortOrder));
        return View(petPosts);
    }

    [HttpGet]
    public async Task<IActionResult> MyPosts(string sortOrder)
    {
        _logger.LogInformation("Show AllPosts..");
        var user = await _userManager.GetUserAsync(User);
        var petPosts = _mapper.Map<IEnumerable<PetPostImageViewModel>>(_petPostImageService
            .FindAllPetPostImageByUser(sortOrder, user.Id));
        return View("AllPosts", petPosts);
    }

    [HttpGet]
    [AllowAnonymous]
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
    [Authorize(Roles="ADMIN")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Deleting post..");
        _postService.ChangeVisibility(id);
        _logger.LogInformation("Post successfully deleted");
        return RedirectToAction(nameof(AllPosts));
    }
}