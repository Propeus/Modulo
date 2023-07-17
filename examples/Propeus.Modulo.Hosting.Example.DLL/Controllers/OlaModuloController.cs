using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Hosting.Example.DLL.Controllers
{
    [Module]
    [Controller]
    [Route("olamundo")]
    public class OlaModuloController : ModuloController
    {

        public OlaModuloController(ILogger<OlaModuloController> logger)
        {
            Logger = logger;
        }

        public ILogger<OlaModuloController> Logger { get; }

        [HttpGet("olamundo2")]
        public IActionResult OlaMundo()
        {
            Logger.LogInformation("Ola Module!!!!!!! - " + Id);
            return Ok("Ola mundo OKView - " + Id);
        }

        [HttpGet(template: "TesteView")]
        public IActionResult Index()
        {
            return View();
        }


    }
}
