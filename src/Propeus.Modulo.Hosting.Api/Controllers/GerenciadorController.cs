using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Hosting;

namespace Propeus.Modulo.Api.Controllers
{
    [Modulo]
    [ApiController]
    [Route("api/gerenciador")]
    public class GerenciadorController : ModuloControllerBase
    {
        private readonly ILogger<GerenciadorController> logger;
        private readonly IGerenciador gerenciador;

        public GerenciadorController(ILogger<GerenciadorController> logger, IGerenciador gerenciador)
        {
            this.logger = logger;
            this.gerenciador = gerenciador;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(gerenciador.Listar().Cast<ModuloBase>());
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (gerenciador.Existe(id))
            {
                return Ok(gerenciador.Obter(id));
            }
            else
            {
                logger.LogWarning("Modulo {id} nao encontrado", id);
                return NotFound();
            }
        }
        [HttpPost("{modulo}")]
        public IActionResult Post(string modulo)
        {
            return Ok(gerenciador.Criar(modulo));
        }
        [HttpPatch("{id}")]
        public IActionResult Patch(string id)
        {
            if (gerenciador.Existe(id))
            {
                return Ok(gerenciador.Reciclar(id));
            }
            else
            {
                return NotFound(id);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (gerenciador.Existe(id))
            {
                gerenciador.Remover(id);
                return NoContent();
            }
            else
            {
                return NotFound(id);
            }
        }
        [HttpDelete]
        public IActionResult Delete()
        {
            gerenciador.RemoverTodos();
            return NoContent();
        }
    }
}
