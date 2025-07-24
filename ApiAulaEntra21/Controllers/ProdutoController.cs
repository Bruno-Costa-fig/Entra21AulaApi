using ApiAulaEntra21.Data;
using Microsoft.AspNetCore.Mvc;
using ApiAulaEntra21.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ApiAulaEntra21.Models.DTO;

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
        
        [HttpGet("loja/{lojaId}")]
        public IActionResult GetByLojaId([FromRoute] int lojaId)
        {
            //var produtos = _context.Produto
            //    .Where(p => p.LojaId == lojaId)
            //    .Include(p => p.Loja)
            //    .ToList();

            var produtos = from produto in _context.Produto
                           join loja in _context.Loja on produto.LojaId equals loja.Id
                           where produto.LojaId == lojaId
                           select new
                           {
                               produto.Id,
                               produto.Nome,
                               produto.Marca,
                               produto.QuantidadeEstoque,
                               LojaId = loja.Id,
                               NomeLoja = loja.Nome
                           };

            if (produtos is null)
            {
                return NotFound("Produtos não encontrado.");
            }

            return Ok(produtos);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] ProdutoDto updatedProduto)
        {
            if (updatedProduto == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            if (string.IsNullOrEmpty(updatedProduto.Nome))
            {
                return BadRequest("O nome do produto é obrigatório.");
            }

            var produto = _context.Produto.FirstOrDefault(p => p.Id == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado.");
            }

            produto.Nome = updatedProduto.Nome;
            produto.Marca = updatedProduto.Marca;
            produto.QuantidadeEstoque = updatedProduto.QuantidadeEstoque;
            produto.LojaId = updatedProduto.LojaId;

            _context.Produto.Update(produto);
            _context.SaveChanges();

            return Ok(produto);
        }


        [HttpPost]
        public IActionResult Post([FromBody] ProdutoDto newProduto)
        {
            if(newProduto == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            if (string.IsNullOrEmpty(newProduto.Nome))
            {
                return BadRequest("O nome do produto é obrigatório.");
            }

            var produto = new Produto()
            {
                LojaId = newProduto.LojaId,
                Nome = newProduto.Nome,
                Marca = newProduto.Marca,
                QuantidadeEstoque = newProduto.QuantidadeEstoque
            };

            _context.Produto.Add(produto);
            _context.SaveChanges();

            return Created("/produto", produto);
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
