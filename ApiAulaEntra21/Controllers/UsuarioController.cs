using ApiAulaEntra21.Data;
using ApiAulaEntra21.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAulaEntra21.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var usuarios = _context.Usuario.Select(x => new
            {
                x.Nome,
                x.Email,
                x.Role
            }).ToList();
            return Ok(usuarios);
        }

        [HttpPost()]
        public IActionResult Registrar(Usuario dto)
        {
            var jaExiste = _context.Usuario
                .Where(x => x.Email == dto.Email).FirstOrDefault();

            if (jaExiste is not null)
                return BadRequest("E-mail já cadastrado.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.SenhaHash),
                Role = dto.Role
            };

            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            return Created("/usuario/", usuario);
        }

    }
}
