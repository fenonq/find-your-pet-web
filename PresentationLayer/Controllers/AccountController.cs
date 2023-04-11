using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(RegisterViewModel model)
    {
        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User is logged in.");

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
            _logger.LogInformation("Error creating user: " + error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", currentUser.Id + ".jpg");
        if (System.IO.File.Exists(path))
        {
            currentUser.PhotoPath = "/userPhotos/" + currentUser.Id + ".jpg";
        }
        else
        {
            currentUser.PhotoPath = "/userPhotos/" + "default.png";
        }

        Console.WriteLine(path);

        _logger.LogInformation("Show account..");
        return View(currentUser);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadUserPhoto(User model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        if (model.Photo.Length <= 0)
        {
            return View("UserProfile", currentUser);
        }

        var fileName = currentUser.Id + Path.GetExtension(model.Photo.FileName);
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "userPhotos", fileName);
        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await model.Photo.CopyToAsync(stream);
        }

        currentUser.PhotoPath = "/userPhotos/" + fileName;
        Console.WriteLine(path);

        return View("UserProfile", currentUser);
    }
}