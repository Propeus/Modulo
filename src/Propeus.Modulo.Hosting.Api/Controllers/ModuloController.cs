using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Logging;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Hosting;

namespace Propeus.Modulo.Api.Controllers
{
    [Modulo]
    [ApiController]
    [Route("api/modulo")]
    public class ModuloController : Hosting.ModuloController
    {
        private readonly ILogger<ModuloController> logger;

        public ModuloController(ILogger<ModuloController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Post(IList<IFormFile> files)
        {
            var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            foreach (var file in files)
            {
                using (var fs = new FileStream(Path.Combine(dir, file.FileName), FileMode.Create))
                {
                    file.CopyTo(fs);
                }
            }
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete(string fileName)
        {
            var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            if (System.IO.File.Exists(Path.Combine(dir, fileName)))
            {
                System.IO.File.Delete(Path.Combine(dir, fileName));
                return NoContent();
            }
            else
            {
                return NotFound(fileName);
            }
        }
        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            if (System.IO.File.Exists(Path.Combine(dir, fileName)))
            {
                using (var fs = new FileStream(Path.Combine(dir, fileName), FileMode.Open))
                {
                    return File(fs, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
            return NotFound(fileName);
        }
    }
}
