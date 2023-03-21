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
        this._logger = logger;
        this._userService = userService;
    }

    public IActionResult Index()
    {
        this._logger.LogInformation("Test information..");
        this._userService.RegisterUser("Taras", "Fenyk", "Taras", "1234");
        this.ViewBag.User = this._userService.FindAll()[0].Name;
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