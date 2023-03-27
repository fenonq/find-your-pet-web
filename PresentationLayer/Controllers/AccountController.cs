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

    public AccountController(ILogger<AccountController> logger, IUserService userService, IMapper mapper)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
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
}