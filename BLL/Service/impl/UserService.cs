using DAL.Model;
using DAL.Repository;
using EmailSender;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using Serilog;
using Serilog.Core;
using System;

namespace BLL.Service.impl;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<UserService> _logger;
    private readonly IEmailService _emailSender;
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<UserService> logger,
        IEmailService emailSender,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
        _contextAccessor = httpContextAccessor;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

    public async Task<bool> LoginUser(string login, string password, bool rememberMe)
    {
        var user = await _userManager.FindByEmailAsync(login);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(
                login,
                password,
                rememberMe,
                lockoutOnFailure: false);

            _logger.LogInformation($"Trying to user log in {login}");

            return result.Succeeded;
        }

        return false;
    }

    public async Task<bool> RegistrateUser(User user, string userPassword)
    {
        var result = await _userManager.CreateAsync(user, userPassword);
        var successSend = false;
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = _urlHelper.Action("ConfirmEmail", "Account", new { token, email = user.Email }, _contextAccessor.HttpContext.Request.Scheme);
            var message = new Message(user.Email, "Completed!", confirmationLink);

            successSend = await _emailSender.SendEmail(message);
        }

        return successSend && result.Succeeded;
    }

    public async Task<bool> ConfirmEmail(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result.Succeeded;
        }

        return true;
    }

    public async Task<bool> ResetPassword(string login, string token, string password)
    {
        var user = await _userManager.FindByEmailAsync(login);

        if (user != null)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }

        return false;
    }

    public async Task<bool> ForgotPassword(string login)
    {
        var user = await _userManager.FindByEmailAsync(login);
        if (user != null && await _userManager.IsEmailConfirmedAsync(user))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetLink = _urlHelper.Action("ResetPassword", "Account", new { email = user.Email, token }, _contextAccessor.HttpContext.Request.Scheme);

            _logger.Log(LogLevel.Warning, passwordResetLink);
            var message = new Message(user.Email, "Reset password", passwordResetLink);

            var successSend = await _emailSender.SendEmail(message);
            return successSend;
        }

        return false;
    }
}