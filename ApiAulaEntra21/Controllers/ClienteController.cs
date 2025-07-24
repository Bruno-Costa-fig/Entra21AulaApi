using ApiAulaEntra21.Data;
using ApiAulaEntra21.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAulaEntra21.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Cliente.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var cliente = _context.Cliente.FirstOrDefault(c => c.Id == id);

            if (cliente is null)
            {
                return BadRequest("Cliente não encontrado!");
            }

            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cliente newCliente)
        {
            _context.Cliente.Add(newCliente);
            _context.SaveChanges();
            return Created("/cliente", newCliente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var cliente = _context.Cliente.FirstOrDefault(c => c.Id == id);

            if(cliente is null)
            {
                return NotFound("Cliente não encontrado.");
            }

            _context.Cliente.Remove(cliente);
            _context.SaveChanges();

            return Ok("Cliente removido com sucesso.");
        }
    }
}
