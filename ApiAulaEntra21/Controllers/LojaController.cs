using ApiAulaEntra21.Data;
using ApiAulaEntra21.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAulaEntra21.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class LojaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LojaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Loja.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var loja = _context.Loja.FirstOrDefault(c => c.Id == id);

            if (loja is null)
            {
                return BadRequest("loja não encontrada!");
            }

            return Ok(loja);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Loja newLoja)
        {
            _context.Loja.Add(newLoja);
            _context.SaveChanges();
            return Created("/loja", newLoja);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var loja = _context.Loja.FirstOrDefault(c => c.Id == id);

            if (loja is null)
            {
                return NotFound("Loja não encontrada.");
            }

            _context.Loja.Remove(loja);
            _context.SaveChanges();

            return Ok("Loja removida com sucesso.");
        }
    }
}
