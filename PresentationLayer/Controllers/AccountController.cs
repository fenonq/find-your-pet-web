using BLL.Service;
using DAL.Model;
using EmailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using System.Security.Cryptography;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace PresentationLayer.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IImageService _imageService;
    private readonly IEmailService _emailSender;


    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        IImageService imageService,
        IEmailService emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _imageService = imageService;
        _emailSender = emailSender;
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
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var userEmail = await _userManager.Users.FirstOrDefaultAsync(a => a.Email == user.Email);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

                var message = new Message(user.Email, "Completed!", confirmationLink);

                _emailSender.SendEmail(message);


                return View("ErrorRegistration");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogInformation("Error creating user: " + error.Description);
            }
        }

        return View(model);
    }


    [HttpGet]
    [AllowAnonymous]

    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return View("Error");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    }

    [HttpGet]
    public IActionResult SuccessRegistration()
    {
        return View();
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
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Email or password is incorrect");
                return View("Error");
            }

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
            else
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View("ErrorEmailConfirmation");
            }
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
        currentUser.PhotoPath = _imageService.GetUserImage(currentUser);

        _logger.LogInformation("Show account..");
        return View(currentUser);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadUserPhoto(User model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        var uploadSuccess = _imageService.UploadUserPhoto(currentUser, model.Photo);
        // !uploadSuccess = add showing error or smth??
        return RedirectToAction("UserProfile", "Account", model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPassword { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var passwordResetLink = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme);

                _logger.Log(LogLevel.Warning, passwordResetLink);


                var message = new Message(user.Email, "Reset password", passwordResetLink);

                _emailSender.SendEmail(message);


                return View("ForgotPasswordConfirmation");
            }

            return View("ForgotPasswordConfirmation");
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordModel(string token, string email)
    {
        // If password reset token or email is null, most likely the
        // user tried to tamper the password reset link
        if (token == null || email == null)
        {
            ModelState.AddModelError(" ", "Invalid password reset token");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPassword model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
                return View(model);
            }

            return View("ResetPasswordConfirmation");
        }
        return View(model);
    }

}