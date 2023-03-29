using AutoMapper;
using BLL.Service;
using BLL.Service.impl;
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
        var current_user = _userService.FindById(2);
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", current_user.Id.ToString() + ".jpg");
        if (System.IO.File.Exists(path))
        {
            current_user.PhotoPath = "/userPhotos/" + current_user.Id.ToString() + ".jpg";
        }
        else
        {
            current_user.PhotoPath = "/userPhotos/" + "default.png";
        }

        _logger.LogInformation("Show account..");
        return View(current_user);
    }

    [HttpPost]
    public IActionResult UploadUserPhoto(User model)
    {
        var current_user = _userService.FindById(2);

        if (model.Photo.Length > 0)
        {
            var fileName = current_user.Id + Path.GetExtension(model.Photo.FileName);
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                model.Photo.CopyTo(stream);
            }

            current_user.PhotoPath = "/userPhotos/" + fileName;
        }

        return View("UserProfile", current_user);
    }
}