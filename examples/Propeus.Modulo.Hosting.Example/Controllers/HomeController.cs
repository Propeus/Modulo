using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Propeus.Modulo.Hosting.Example.Models;
using Propeus.Modulo.Hosting.Example.Modules;

namespace Propeus.Modulo.Hosting.Example.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OlaMundoHttpModulo olaMundoHttpModulo;

    public HomeController(ILogger<HomeController> logger, IServiceProvider provider, OlaMundoHttpModulo olaMundoHttpModulo)
    {
        _logger = logger;
        //this.olaMundoHttpModulo = olaMundoHttpModulo;
    }

    public IActionResult Index()
    {
        //building Web apps with ASP.NET Core
        //ViewData["Modulo"] = olaMundoHttpModulo.Executar();
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
