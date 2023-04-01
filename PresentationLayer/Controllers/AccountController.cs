using AutoMapper;
using BLL.Service;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(
        ILogger<AccountController> logger,
        IUserService userService,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Registration()
    {
        _logger.LogInformation("Show registration form..");
        return View();
    }

    [HttpPost]
    public IActionResult Registration(RegisterViewModel model)
    {
        _logger.LogInformation("Registering user..");
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Wrong input data!");
            return View(model);
        }

        _userService.Add(_mapper.Map<User>(model));
        _logger.LogInformation("Successfully registered");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult UserProfile()
    {
        var currentUser = _userService.FindById(2);
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", currentUser.Id + ".jpg");
        if (System.IO.File.Exists(path))
        {
            currentUser.PhotoPath = "/userPhotos/" + currentUser.Id + ".jpg";
        }
        else
        {
            currentUser.PhotoPath = "/userPhotos/" + "default.png";
        }

        _logger.LogInformation("Show account..");
        return View(currentUser);
    }

    [HttpPost]
    public IActionResult UploadUserPhoto(User model)
    {
        var currentUser = _userService.FindById(2);

        if (model.Photo.Length <= 0)
        {
            return View("UserProfile", currentUser);
        }

        var fileName = currentUser.Id + Path.GetExtension(model.Photo.FileName);
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            model.Photo.CopyTo(stream);
        }

        currentUser.PhotoPath = "/userPhotos/" + fileName;

        return View("UserProfile", currentUser);
    }
}