using DAL.Model;

namespace PresentationLayer.Controllers;

using System.Diagnostics;
using BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Test information..");
        _userService.Add(new User
        {
            Name = "test",
            Surname = "test",
            Login = "test",
            Password = "test",
        });
        ViewBag.User = _userService.FindAll()[0].Name;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}