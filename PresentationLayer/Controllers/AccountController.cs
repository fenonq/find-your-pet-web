using BLL.Service;
using DAL.Model;
using EmailSender;
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
    private readonly IImageService _imageService;
    private readonly IEmailService _emailSender;
    private readonly IUserService _userService;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        IImageService imageService,
        IEmailService emailSender,
        IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _imageService = imageService;
        _emailSender = emailSender;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
            };
            var result = await _userService.RegistrateUser(user, model.Password);

            return result ? View("Info", new UserInfo("Registration successful!", "Please confirm your email, by clicking on the confirmation link we emailed you"))
                          : View("Error");
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var result = await _userService.ConfirmEmail(email, token);
        if (result)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return View("Info", new UserInfo("Welcome in our team!", "Thank you for confirming your email"));
        }

        return View("Error");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.LoginUser(model.Email, model.Password, model.RememberMe);
            return result ? RedirectToAction("Index", "Home") : View("Error");
        }

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

        if (currentUser != null)
        {
            currentUser.PhotoPath = _imageService.GetUserImage(currentUser);
        }

        _logger.LogInformation("Show account..");
        return View(currentUser);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadUserPhoto(User model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        var uploadSuccess = _imageService.UploadUserPhoto(currentUser, model.Photo);
        return RedirectToAction("UserProfile", "Account", model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.ForgotPassword(model.Email);
            return result ? View("Info", new UserInfo("The final steps!", "We have sent an email with the instructions to reset your password")) : View("Error");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPassword model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.ResetPassword(model.Email, model.Token, model.Password);
            return result ? View("ResetPasswordConfirmation") : View("Error");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPassword { Token = token, Email = email };
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

}