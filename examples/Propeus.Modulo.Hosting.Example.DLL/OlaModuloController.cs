using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Propeus.Modulo.Abstrato.Atributos;

namespace Propeus.Modulo.Hosting.Example.Modules
{
    [Modulo]
    public class OlaModuloController : ModuloController
    {

        public OlaModuloController(ILogger<OlaModuloController> logger)
        {
            Logger = logger;
        }

        public ILogger<OlaModuloController> Logger { get; }

        [HttpGet]
        public IActionResult OlaMundo()
        {
            Logger.LogInformation("Ola Modulo!!! - " + this.Id);
            return Ok("Ola Modulo - " + this.Id);
        }
    }
}
