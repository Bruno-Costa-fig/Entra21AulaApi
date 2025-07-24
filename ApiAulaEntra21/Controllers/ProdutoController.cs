using ApiAulaEntra21.Data;
using Microsoft.AspNetCore.Mvc;
using ApiAulaEntra21.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiAulaEntra21.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Produto.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var produto = _context.Produto.FirstOrDefault(p => p.Id == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado.");
            }
            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto newProduto)
        {
            if(newProduto == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            if (string.IsNullOrEmpty(newProduto.Nome))
            {
                return BadRequest("O nome do produto é obrigatório.");
            }

            _context.Produto.Add(newProduto);
            _context.SaveChanges();

            return Created("/produto", newProduto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var produto = _context.Produto.FirstOrDefault(p => p.Id == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado.");
            }

            _context.Produto.Remove(produto);
            _context.SaveChanges();
            return Ok("Produto removido com sucesso!");
        }
    }
}
