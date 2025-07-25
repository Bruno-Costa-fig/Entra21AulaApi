using ApiAulaEntra21.Data;
using Microsoft.AspNetCore.Mvc;
using ApiAulaEntra21.Models;
using ApiAulaEntra21.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ApiAulaEntra21.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
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
        public IActionResult GetPorLojaId([FromRoute] int lojaId)
        {
            //return Ok(_context.Produto
            //    .Where(x => x.LojaId == lojaId)
            //    .Include(p => p.Loja)
            //    .Select(p => new
            //    {
            //        NomeProduto = p.Nome,
            //        Quantidade = p.QuantidadeEstoque,
            //        Marca = p.Marca,
            //        NomeLoja = p.Loja.Nome
            //    })
            //    .ToList());

            var produtos = from produto in _context.Produto
                           join loja in _context.Loja
                           on produto.LojaId equals loja.Id
                           where produto.LojaId == lojaId
                           select new
                           {
                               NomeProduto = produto.Nome,
                               Quantidade = produto.QuantidadeEstoque,
                               Marca = produto.Marca,
                               NomeLoja = loja.Nome
                           };

            return Ok(produtos);
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
                Nome = newProduto.Nome,
                Marca = newProduto.Marca,
                QuantidadeEstoque = newProduto.QuantidadeEstoque,
                LojaId = newProduto.LojaId
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
